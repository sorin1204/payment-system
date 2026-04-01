namespace TMPPP.Domain.Behavioral.Command;

public sealed record PaymentCommandState(
    bool Authorized,
    bool Captured,
    bool Refunded,
    string Status,
    IReadOnlyCollection<string> AuditTrail);
