namespace TMPPP.Domain.Behavioral.Mediator;

public abstract class PaymentParticipant
{
    private readonly List<string> _handledEvents = [];

    protected PaymentParticipant(string name)
    {
        Name = name;
        Status = "idle";
    }

    protected IPaymentWorkflowMediator Mediator { get; private set; } = null!;
    public string Name { get; }
    public string Status { get; protected set; }

    public void SetMediator(IPaymentWorkflowMediator mediator)
    {
        Mediator = mediator;
    }

    protected void Track(string eventName, string status)
    {
        _handledEvents.Add(eventName);
        Status = status;
    }

    public PaymentMediatorParticipantSnapshot Snapshot()
    {
        return new PaymentMediatorParticipantSnapshot(Name, Status, _handledEvents.ToList());
    }
}
