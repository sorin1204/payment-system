namespace TMPPP.Domain.Structural.Adapter;

public sealed record PaymentResponse(bool Success, string Provider, string TransactionId, string Message);
