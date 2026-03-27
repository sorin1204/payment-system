namespace TMPPP.Domain.Structural.Decorator;

public sealed class PushNotificationDecorator : NotificationServiceDecorator
{
    public PushNotificationDecorator(
        TMPPP.Domain.Interfaces.INotificationService inner,
        DecoratorNotificationContext context)
        : base(inner, context)
    {
    }

    public override void Notify(string recipient, string subject, string message)
    {
        base.Notify(recipient, subject, message);
        Context.Record("Push", $"Push notification queued for {recipient}: {subject}");
    }
}
