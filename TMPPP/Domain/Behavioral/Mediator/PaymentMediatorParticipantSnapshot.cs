namespace TMPPP.Domain.Behavioral.Mediator;

public sealed record PaymentMediatorParticipantSnapshot(
    string Participant,
    string Status,
    IReadOnlyCollection<string> HandledEvents);
