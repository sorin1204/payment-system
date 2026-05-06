namespace TMPPP.Domain.Behavioral.Mediator;

public sealed class PaymentCoordinationContext
{
    public PaymentCoordinationContext(string paymentReference, decimal amount, string currency, string method, PaymentFraudDecision fraudDecision)
    {
        PaymentReference = paymentReference;
        Amount = amount;
        Currency = currency;
        Method = method;
        FraudDecision = fraudDecision;
        CurrentStatus = "created";
    }

    public string PaymentReference { get; }
    public decimal Amount { get; }
    public string Currency { get; }
    public string Method { get; }
    public PaymentFraudDecision FraudDecision { get; }
    public string CurrentStatus { get; set; }
    public bool FraudReviewTriggered { get; set; }
    public string? CustomerMessage { get; set; }
    public List<PaymentMediatorLogEntry> Timeline { get; } = [];

    public void AddTimeline(string from, string to, string eventName, string message)
    {
        Timeline.Add(new PaymentMediatorLogEntry(from, to, eventName, message));
    }
}
