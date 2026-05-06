namespace TMPPP.Domain.Behavioral.Mediator;

public sealed class FraudReviewDesk : PaymentParticipant
{
    public FraudReviewDesk()
        : base("Fraud review desk")
    {
    }

    public void Review(PaymentCoordinationContext context)
    {
        context.FraudReviewTriggered = true;

        if (context.FraudDecision == PaymentFraudDecision.Reject)
        {
            Track("fraud-rejected", "rejected");
            context.AddTimeline(Name, "Mediator", "fraud-rejected", "Echipa de risc a respins plata dupa verificare.");
            Mediator.Notify(this, "fraud-rejected", context);
            return;
        }

        Track("fraud-approved", "approved");
        context.AddTimeline(Name, "Mediator", "fraud-approved", "Echipa de risc a aprobat plata pentru pasul contabil.");
        Mediator.Notify(this, "fraud-approved", context);
    }
}
