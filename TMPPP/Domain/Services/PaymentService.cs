using TMPPP.Domain.Behavioral.Chain;
using TMPPP.Domain.Entities;
using TMPPP.Domain.Interfaces;
using TMPPP.Domain.ValueObjects;

namespace TMPPP.Domain.Services;

public sealed class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentHandler _paymentProcessingChain;

    public PaymentService(
        IPaymentRepository paymentRepository,
        IInvoiceRepository invoiceRepository,
        IPaymentProcessor paymentProcessor)
    {
        _paymentRepository = paymentRepository;
        var loadPayment = new LoadPaymentHandler(paymentRepository);
        var validateInvoice = new InvoiceValidationHandler(invoiceRepository);
        var validatePending = new PendingPaymentHandler();
        var validateMethod = new PaymentMethodSupportHandler();
        var executePayment = new PaymentExecutionHandler(paymentProcessor);

        loadPayment.SetNext(validateInvoice)
            .SetNext(validatePending)
            .SetNext(validateMethod)
            .SetNext(executePayment);

        _paymentProcessingChain = loadPayment;
    }

    public Payment CreatePayment(Guid invoiceId, Money amount)
    {
        var payment = new Payment(Guid.NewGuid(), invoiceId, amount, DateTime.UtcNow);
        _paymentRepository.Add(payment);
        return payment;
    }

    public PaymentResult ProcessPayment(Guid paymentId, IPaymentMethod method)
    {
        var context = new PaymentChainContext(paymentId, method);
        return _paymentProcessingChain.Handle(context);
    }
}
