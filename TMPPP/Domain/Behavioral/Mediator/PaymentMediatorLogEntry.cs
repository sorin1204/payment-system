namespace TMPPP.Domain.Behavioral.Mediator;

public sealed record PaymentMediatorLogEntry(string From, string To, string Event, string Message);
