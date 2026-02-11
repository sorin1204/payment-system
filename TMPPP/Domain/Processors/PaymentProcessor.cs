using TMPPP.Domain.Entities;
using TMPPP.Domain.Interfaces;

namespace TMPPP.Domain.Processors;

public sealed class PaymentProcessor : IPaymentProcessor
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly INotificationService _notificationService;

    public PaymentProcessor(IPaymentRepository paymentRepository, INotificationService notificationService)
    {
        _paymentRepository = paymentRepository;
        _notificationService = notificationService;
    }

    public PaymentResult Process(Payment payment, IPaymentMethod method)
    {
        if (!method.Supports(payment.Amount))
        {
            payment.MarkFailed();
            _paymentRepository.Update(payment);
            return new PaymentResult(false, "Payment method does not support amount.");
        }

        var result = method.Process(payment);
        if (result.Success)
        {
            payment.MarkProcessed();
        }
        else
        {
            payment.MarkFailed();
        }

        _paymentRepository.Update(payment);
        _notificationService.Notify(payment.InvoiceId.ToString(), "Payment status", result.Message);
        return result;
    }
}
