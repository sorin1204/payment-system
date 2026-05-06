namespace TMPPP.Domain.Behavioral.Mediator;

public sealed class AccountingDesk : PaymentParticipant
{
    public AccountingDesk()
        : base("Accounting desk")
    {
    }

    public void Record(PaymentCoordinationContext context)
    {
        Track("accounting-recorded", "recorded");
        context.AddTimeline(Name, "Mediator", "accounting-recorded", $"Contabilitatea a inregistrat plata de {context.Amount} {context.Currency}.");
        Mediator.Notify(this, "accounting-recorded", context);
    }
}
