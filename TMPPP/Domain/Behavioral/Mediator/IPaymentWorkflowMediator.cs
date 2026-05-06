namespace TMPPP.Domain.Behavioral.Mediator;

public interface IPaymentWorkflowMediator
{
    void Notify(PaymentParticipant sender, string eventName, PaymentCoordinationContext context);
}
