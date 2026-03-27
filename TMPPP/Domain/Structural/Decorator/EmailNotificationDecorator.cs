namespace TMPPP.Domain.Structural.Decorator;

public sealed class EmailNotificationDecorator : NotificationServiceDecorator
{
    public EmailNotificationDecorator(
        TMPPP.Domain.Interfaces.INotificationService inner,
        DecoratorNotificationContext context)
        : base(inner, context)
    {
    }

    public override void Notify(string recipient, string subject, string message)
    {
        base.Notify(recipient, subject, message);
        Context.Record("Email", $"Email sent to {recipient} with subject '{subject}'.");
    }
}
