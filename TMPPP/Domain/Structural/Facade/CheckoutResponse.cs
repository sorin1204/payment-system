namespace TMPPP.Domain.Structural.Facade;

public sealed record CheckoutResponse(
    Guid CustomerId,
    Guid InvoiceId,
    Guid PaymentId,
    bool Success,
    string Message,
    string PaymentMethod,
    decimal Amount,
    string Currency);
