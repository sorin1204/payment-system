namespace TMPPP.Domain.Entities;

public sealed class PaymentResult
{
    public PaymentResult(
        bool success,
        string message,
        string? failureCode = null,
        IReadOnlyList<TMPPP.Domain.Behavioral.Chain.PaymentChainStep>? chainTrace = null)
    {
        Success = success;
        Message = message;
        FailureCode = failureCode;
        ChainTrace = chainTrace ?? [];
    }

    public bool Success { get; }
    public string Message { get; }
    public string? FailureCode { get; }
    public IReadOnlyList<TMPPP.Domain.Behavioral.Chain.PaymentChainStep> ChainTrace { get; }
}
