using TMPPP.Domain.Entities;
using TMPPP.Domain.Factories;
using TMPPP.Domain.Interfaces;
using TMPPP.Domain.Services;
using TMPPP.Domain.ValueObjects;

namespace TMPPP.Domain.Structural.Facade;

public class PaymentCheckoutFacade
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IPaymentService _paymentService;

    public PaymentCheckoutFacade(
        ICustomerRepository customerRepository,
        IInvoiceRepository invoiceRepository,
        IPaymentService paymentService)
    {
        _customerRepository = customerRepository;
        _invoiceRepository = invoiceRepository;
        _paymentService = paymentService;
    }

    public CheckoutResponse ExecuteCheckout(CheckoutRequest request)
    {
        ValidateRequest(request);

        var currency = request.Currency.Trim().ToUpperInvariant();
        var customerId = Guid.NewGuid();
        _customerRepository.EnsureExists(customerId, request.CustomerName.Trim(), request.CustomerEmail.Trim());

        var invoice = new Invoice(
            Guid.NewGuid(),
            customerId,
            new Money(request.Amount, currency),
            request.DueDateUtc ?? DateTime.UtcNow.AddDays(14));
        _invoiceRepository.Add(invoice);

        var payment = _paymentService.CreatePayment(invoice.Id, new Money(request.Amount, currency));
        var method = ResolveCreator(request.PaymentMethod).CreatePaymentMethod();
        var result = _paymentService.ProcessPayment(payment.Id, method);

        return new CheckoutResponse(
            customerId,
            invoice.Id,
            payment.Id,
            result.Success,
            result.Message,
            method.MethodName,
            request.Amount,
            currency);
    }

    private static void ValidateRequest(CheckoutRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CustomerName))
        {
            throw new ArgumentException("CustomerName is required.");
        }

        if (string.IsNullOrWhiteSpace(request.CustomerEmail))
        {
            throw new ArgumentException("CustomerEmail is required.");
        }

        if (request.Amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than 0.");
        }

        if (string.IsNullOrWhiteSpace(request.Currency))
        {
            throw new ArgumentException("Currency is required.");
        }

        if (string.IsNullOrWhiteSpace(request.PaymentMethod))
        {
            throw new ArgumentException("PaymentMethod is required.");
        }
    }

    private static PaymentMethodCreator ResolveCreator(string methodChoice)
    {
        return methodChoice.Trim().ToLowerInvariant() switch
        {
            "1" or "card" => new CardPaymentMethodCreator(),
            "2" or "bank" or "banktransfer" => new BankTransferPaymentMethodCreator(),
            "3" or "cash" => new CashPaymentMethodCreator(),
            _ => throw new ArgumentException("Method must be one of: card, bank, cash.")
        };
    }
}
