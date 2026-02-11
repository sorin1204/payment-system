using TMPPP.Domain.Entities;
using TMPPP.Domain.Interfaces;
using TMPPP.Domain.ValueObjects;

namespace TMPPP.Domain.Services;

public sealed class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IPaymentProcessor _paymentProcessor;

    public PaymentService(
        IPaymentRepository paymentRepository,
        IInvoiceRepository invoiceRepository,
        IPaymentProcessor paymentProcessor)
    {
        _paymentRepository = paymentRepository;
        _invoiceRepository = invoiceRepository;
        _paymentProcessor = paymentProcessor;
    }

    public Payment CreatePayment(Guid invoiceId, Money amount)
    {
        var payment = new Payment(Guid.NewGuid(), invoiceId, amount, DateTime.UtcNow);
        _paymentRepository.Add(payment);
        return payment;
    }

    public PaymentResult ProcessPayment(Guid paymentId, IPaymentMethod method)
    {
        var payment = _paymentRepository.GetById(paymentId)
            ?? throw new InvalidOperationException("Payment not found.");

        var invoice = _invoiceRepository.GetById(payment.InvoiceId)
            ?? throw new InvalidOperationException("Invoice not found.");

        _invoiceRepository.Update(invoice);
        return _paymentProcessor.Process(payment, method);
    }
}
