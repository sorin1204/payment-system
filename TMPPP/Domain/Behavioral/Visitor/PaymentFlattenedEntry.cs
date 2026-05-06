namespace TMPPP.Domain.Behavioral.Visitor;

public sealed record PaymentFlattenedEntry(string Path, decimal Amount, string Currency);
