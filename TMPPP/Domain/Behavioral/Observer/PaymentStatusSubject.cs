using TMPPP.Domain.Entities;
using TMPPP.Domain.Enums;

namespace TMPPP.Domain.Behavioral.Observer;

public sealed class PaymentStatusSubject
{
    private readonly List<IPaymentStatusObserver> _observers = [];

    public PaymentStatusSubject(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public IReadOnlyCollection<IPaymentStatusObserver> Observers => _observers;

    public void Attach(IPaymentStatusObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IPaymentStatusObserver observer)
    {
        _observers.Remove(observer);
    }

    public PaymentStatusChangedEvent ChangeStatus(Payment payment, PaymentStatus nextStatus, string changedBy)
    {
        var previousStatus = payment.Status;

        ApplyStatus(payment, nextStatus);

        var statusChangedEvent = new PaymentStatusChangedEvent(
            payment.Id.ToString("N")[..8].ToUpperInvariant(),
            previousStatus,
            payment.Status,
            payment.Amount.Amount,
            payment.Amount.Currency,
            DateTime.UtcNow,
            changedBy);

        foreach (var observer in _observers)
        {
            observer.Update(payment, statusChangedEvent);
        }

        return statusChangedEvent;
    }

    private static void ApplyStatus(Payment payment, PaymentStatus nextStatus)
    {
        switch (nextStatus)
        {
            case PaymentStatus.Pending:
                return;
            case PaymentStatus.Processed:
                payment.MarkProcessed();
                return;
            case PaymentStatus.Failed:
                payment.MarkFailed();
                return;
            case PaymentStatus.Refunded:
                if (payment.Status == PaymentStatus.Pending)
                {
                    payment.MarkProcessed();
                }

                payment.MarkRefunded();
                return;
            default:
                throw new ArgumentOutOfRangeException(nameof(nextStatus), nextStatus, "Unsupported payment status.");
        }
    }
}
