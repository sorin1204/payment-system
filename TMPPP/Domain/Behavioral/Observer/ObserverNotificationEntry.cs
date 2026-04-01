namespace TMPPP.Domain.Behavioral.Observer;

public sealed class ObserverNotificationEntry
{
    public ObserverNotificationEntry(string observer, string destination, string message)
    {
        Observer = observer;
        Destination = destination;
        Message = message;
    }

    public string Observer { get; }
    public string Destination { get; }
    public string Message { get; }
}
