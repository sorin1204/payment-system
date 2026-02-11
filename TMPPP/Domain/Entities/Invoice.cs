using TMPPP.Domain.ValueObjects;

namespace TMPPP.Domain.Entities;

public sealed class Invoice
{
    public Invoice(Guid id, Guid customerId, Money total, DateTime dueDate)
    {
        Id = id;
        CustomerId = customerId;
        Total = total;
        DueDate = dueDate;
    }

    public Guid Id { get; }
    public Guid CustomerId { get; }
    public Money Total { get; private set; }
    public DateTime DueDate { get; private set; }

    public void UpdateTotal(Money total)
    {
        Total = total;
    }

    public void Reschedule(DateTime dueDate)
    {
        DueDate = dueDate;
    }
}
