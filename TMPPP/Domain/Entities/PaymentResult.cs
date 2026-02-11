namespace TMPPP.Domain.Entities;

public sealed class PaymentResult
{
    public PaymentResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public bool Success { get; }
    public string Message { get; }
}
