using TMPPP.Domain.Entities;
using TMPPP.Domain.Enums;
using TMPPP.Domain.Factories;
using TMPPP.Domain.Interfaces;
using TMPPP.Domain.Processors;
using TMPPP.Domain.Services;
using TMPPP.Domain.ValueObjects;
using TMPPP.Domain.Behavioral.State;

namespace TMPPP.Controllers;

public static class ChainController
{
    public static PaymentResult BuildPaymentChainDemo(
        bool paymentExists,
        bool invoiceExists,
        PaymentStatus initialStatus,
        decimal amount,
        string currency,
        string method)
    {
        var invoiceId = Guid.NewGuid();
        var paymentId = Guid.NewGuid();
        var paymentRepository = new DemoPaymentRepository();
        var invoiceRepository = new DemoInvoiceRepository();

        if (invoiceExists)
        {
            invoiceRepository.Add(new Invoice(invoiceId, Guid.NewGuid(), new Money(Math.Abs(amount), currency), DateTime.UtcNow.AddDays(7)));
        }

        if (paymentExists)
        {
            var payment = new Payment(paymentId, invoiceId, new Money(amount, currency), DateTime.UtcNow);
            ApplyStatus(payment, initialStatus);
            paymentRepository.Add(payment);
        }

        var paymentProcessor = new PaymentProcessor(paymentRepository, new DemoNotificationService());
        var paymentService = new PaymentService(paymentRepository, invoiceRepository, paymentProcessor);
        var paymentMethod = ResolveCreator(method).CreatePaymentMethod();

        return paymentService.ProcessPayment(paymentId, paymentMethod);
    }

    private static void ApplyStatus(Payment payment, PaymentStatus status)
    {
        switch (status)
        {
            case PaymentStatus.Pending:
                return;
            case PaymentStatus.Processed:
                payment.ForceState(PaymentStateFactory.Create(PaymentStatus.Processed));
                break;
            case PaymentStatus.Failed:
                payment.ForceState(PaymentStateFactory.Create(PaymentStatus.Failed));
                break;
            case PaymentStatus.Refunded:
                payment.ForceState(PaymentStateFactory.Create(PaymentStatus.Refunded));
                break;
        }
    }

    private static PaymentMethodCreator ResolveCreator(string? methodChoice)
    {
        return methodChoice?.Trim().ToLowerInvariant() switch
        {
            "1" or "card" => new CardPaymentMethodCreator(),
            "2" or "bank" or "banktransfer" => new BankTransferPaymentMethodCreator(),
            "3" or "cash" => new CashPaymentMethodCreator(),
            _ => throw new ArgumentException("Method must be one of: card, bank, cash.")
        };
    }

    private sealed class DemoPaymentRepository : IPaymentRepository
    {
        private readonly Dictionary<Guid, Payment> _payments = [];

        public Payment? GetById(Guid id)
        {
            return _payments.TryGetValue(id, out var payment) ? payment : null;
        }

        public void Add(Payment payment)
        {
            _payments[payment.Id] = payment;
        }

        public void Update(Payment payment)
        {
            _payments[payment.Id] = payment;
        }
    }

    private sealed class DemoInvoiceRepository : IInvoiceRepository
    {
        private readonly Dictionary<Guid, Invoice> _invoices = [];

        public Invoice? GetById(Guid id)
        {
            return _invoices.TryGetValue(id, out var invoice) ? invoice : null;
        }

        public void Add(Invoice invoice)
        {
            _invoices[invoice.Id] = invoice;
        }

        public void Update(Invoice invoice)
        {
            _invoices[invoice.Id] = invoice;
        }

        public IReadOnlyCollection<Invoice> GetAll()
        {
            return _invoices.Values.ToList();
        }
    }

    private sealed class DemoNotificationService : INotificationService
    {
        public void Notify(string recipient, string subject, string message)
        {
        }
    }
}
