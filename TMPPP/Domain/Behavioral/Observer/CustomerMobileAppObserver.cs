using TMPPP.Domain.Entities;

namespace TMPPP.Domain.Behavioral.Observer;

public sealed class CustomerMobileAppObserver : IPaymentStatusObserver
{
    private readonly ObserverNotificationJournal _journal;

    public CustomerMobileAppObserver(ObserverNotificationJournal journal)
    {
        _journal = journal;
    }

    public string Name => "Customer mobile app";

    public void Update(Payment payment, PaymentStatusChangedEvent statusChangedEvent)
    {
        _journal.Add(
            Name,
            "customer-app",
            $"Payment {statusChangedEvent.PaymentReference} changed from {statusChangedEvent.PreviousStatus} to {statusChangedEvent.CurrentStatus}.");
    }
}
