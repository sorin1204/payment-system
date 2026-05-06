namespace TMPPP.Domain.Behavioral.Mediator;

public sealed class CustomerNotificationDesk : PaymentParticipant
{
    public CustomerNotificationDesk()
        : base("Customer notification desk")
    {
    }

    public void Send(PaymentCoordinationContext context, string message)
    {
        Track("customer-notified", "sent");
        context.CustomerMessage = message;
        context.AddTimeline(Name, "Mediator", "customer-notified", message);
        Mediator.Notify(this, "customer-notified", context);
    }
}
