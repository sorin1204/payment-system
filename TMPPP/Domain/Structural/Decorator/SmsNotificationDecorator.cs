namespace TMPPP.Domain.Structural.Decorator;

public sealed class SmsNotificationDecorator : NotificationServiceDecorator
{
    public SmsNotificationDecorator(
        TMPPP.Domain.Interfaces.INotificationService inner,
        DecoratorNotificationContext context)
        : base(inner, context)
    {
    }

    public override void Notify(string recipient, string subject, string message)
    {
        base.Notify(recipient, subject, message);
        Context.Record("SMS", $"SMS fallback sent for recipient {recipient}: {message}");
    }
}
