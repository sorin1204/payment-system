namespace TMPPP.Domain.Structural.Facade;

public sealed record CheckoutRequest(
    string CustomerName,
    string CustomerEmail,
    decimal Amount,
    string Currency,
    string PaymentMethod,
    DateTime? DueDateUtc);
