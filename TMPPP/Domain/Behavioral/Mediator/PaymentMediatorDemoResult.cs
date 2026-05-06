namespace TMPPP.Domain.Behavioral.Mediator;

public sealed record PaymentMediatorDemoResult(
    string PaymentReference,
    decimal Amount,
    string Currency,
    string Method,
    string FinalStatus,
    bool FraudReviewTriggered,
    string? CustomerMessage,
    IReadOnlyCollection<PaymentMediatorLogEntry> Timeline,
    IReadOnlyCollection<PaymentMediatorParticipantSnapshot> Participants,
    string Explanation);
