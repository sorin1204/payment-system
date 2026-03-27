namespace TMPPP.Domain.Structural.Decorator;

public sealed class DecoratorNotificationContext
{
    private readonly List<NotificationChannel> _deliveredChannels = [];

    public IReadOnlyCollection<NotificationChannel> DeliveredChannels => _deliveredChannels;

    public void Record(string name, string details)
    {
        _deliveredChannels.Add(new NotificationChannel(name, details));
    }
}
