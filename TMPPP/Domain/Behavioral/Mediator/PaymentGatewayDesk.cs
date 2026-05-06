namespace TMPPP.Domain.Behavioral.Mediator;

public sealed class PaymentGatewayDesk : PaymentParticipant
{
    public PaymentGatewayDesk()
        : base("Payment gateway desk")
    {
    }

    public void Submit(PaymentCoordinationContext context)
    {
        Track("payment-submitted", "submitted");
        context.CurrentStatus = "submitted";
        context.AddTimeline(Name, "Mediator", "payment-submitted", $"Plata {context.PaymentReference} a fost trimisa pentru coordonare.");
        Mediator.Notify(this, "payment-submitted", context);
    }

    public void MoveToReview(PaymentCoordinationContext context)
    {
        Track("payment-routed-to-review", "under-review");
        context.CurrentStatus = "under-review";
        context.AddTimeline("Mediator", Name, "payment-routed-to-review", "Mediatorul a trimis plata catre verificarea de risc.");
    }

    public void MarkApproved(PaymentCoordinationContext context)
    {
        Track("fraud-approved", "approved");
        context.CurrentStatus = "approved";
        context.AddTimeline("Mediator", Name, "fraud-approved", "Mediatorul a primit aprobarea de risc si a deblocat contabilizarea.");
    }

    public void MarkRejected(PaymentCoordinationContext context)
    {
        Track("fraud-rejected", "rejected");
        context.CurrentStatus = "rejected";
        context.AddTimeline("Mediator", Name, "fraud-rejected", "Mediatorul a blocat plata dupa respingerea de risc.");
    }

    public void MarkBooked(PaymentCoordinationContext context)
    {
        Track("accounting-recorded", "booked");
        context.CurrentStatus = "booked";
        context.AddTimeline("Mediator", Name, "accounting-recorded", "Plata a fost confirmata dupa inregistrarea in contabilitate.");
    }
}
