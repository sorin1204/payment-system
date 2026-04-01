using TMPPP.Domain.Enums;

namespace TMPPP.Domain.Behavioral.Observer;

public sealed class PaymentStatusChangedEvent
{
    public PaymentStatusChangedEvent(
        string paymentReference,
        PaymentStatus previousStatus,
        PaymentStatus currentStatus,
        decimal amount,
        string currency,
        DateTime changedAtUtc,
        string changedBy)
    {
        PaymentReference = paymentReference;
        PreviousStatus = previousStatus;
        CurrentStatus = currentStatus;
        Amount = amount;
        Currency = currency;
        ChangedAtUtc = changedAtUtc;
        ChangedBy = changedBy;
    }

    public string PaymentReference { get; }
    public PaymentStatus PreviousStatus { get; }
    public PaymentStatus CurrentStatus { get; }
    public decimal Amount { get; }
    public string Currency { get; }
    public DateTime ChangedAtUtc { get; }
    public string ChangedBy { get; }
}
