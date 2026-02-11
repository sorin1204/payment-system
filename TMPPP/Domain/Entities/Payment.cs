using TMPPP.Domain.Enums;
using TMPPP.Domain.ValueObjects;

namespace TMPPP.Domain.Entities;

public sealed class Payment
{
    public Payment(Guid id, Guid invoiceId, Money amount, DateTime createdAt)
    {
        Id = id;
        InvoiceId = invoiceId;
        Amount = amount;
        CreatedAt = createdAt;
        Status = PaymentStatus.Pending;
    }

    public Guid Id { get; }
    public Guid InvoiceId { get; }
    public Money Amount { get; }
    public DateTime CreatedAt { get; }
    public PaymentStatus Status { get; private set; }

    public void MarkProcessed()
    {
        Status = PaymentStatus.Processed;
    }

    public void MarkFailed()
    {
        Status = PaymentStatus.Failed;
    }

    public void MarkRefunded()
    {
        Status = PaymentStatus.Refunded;
    }
}
