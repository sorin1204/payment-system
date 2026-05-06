using TMPPP.Domain.Enums;

namespace TMPPP.Domain.Behavioral.State;

public static class PaymentStateFactory
{
    public static IPaymentState Create(PaymentStatus status)
    {
        return status switch
        {
            PaymentStatus.Pending => new PendingPaymentState(),
            PaymentStatus.Processed => new ProcessedPaymentState(),
            PaymentStatus.Failed => new FailedPaymentState(),
            PaymentStatus.Refunded => new RefundedPaymentState(),
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Unsupported payment state.")
        };
    }
}
