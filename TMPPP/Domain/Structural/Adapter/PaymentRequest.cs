namespace TMPPP.Domain.Structural.Adapter;

public sealed record PaymentRequest(decimal Amount, string Currency, string Description);
