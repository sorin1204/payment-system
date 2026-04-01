using TMPPP.Domain.Enums;

namespace TMPPP.Domain.Behavioral.Observer;

public sealed class ObserverDemoResult
{
    public ObserverDemoResult(
        string subjectName,
        string paymentReference,
        PaymentStatus previousStatus,
        PaymentStatus currentStatus,
        IReadOnlyCollection<string> observers,
        IReadOnlyCollection<ObserverNotificationEntry> notifications,
        string explanation)
    {
        SubjectName = subjectName;
        PaymentReference = paymentReference;
        PreviousStatus = previousStatus;
        CurrentStatus = currentStatus;
        Observers = observers;
        Notifications = notifications;
        Explanation = explanation;
    }

    public string SubjectName { get; }
    public string PaymentReference { get; }
    public PaymentStatus PreviousStatus { get; }
    public PaymentStatus CurrentStatus { get; }
    public IReadOnlyCollection<string> Observers { get; }
    public IReadOnlyCollection<ObserverNotificationEntry> Notifications { get; }
    public int ObserverCount => Observers.Count;
    public string Explanation { get; }
}
