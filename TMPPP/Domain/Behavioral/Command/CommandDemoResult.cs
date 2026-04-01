namespace TMPPP.Domain.Behavioral.Command;

public sealed class CommandDemoResult
{
    public CommandDemoResult(
        string paymentReference,
        decimal amount,
        string currency,
        IReadOnlyCollection<string> queuedCommands,
        IReadOnlyCollection<PaymentCommandExecutionRecord> executionLog,
        PaymentCommandState finalState,
        string explanation)
    {
        PaymentReference = paymentReference;
        Amount = amount;
        Currency = currency;
        QueuedCommands = queuedCommands;
        ExecutionLog = executionLog;
        FinalState = finalState;
        Explanation = explanation;
    }

    public string PaymentReference { get; }
    public decimal Amount { get; }
    public string Currency { get; }
    public IReadOnlyCollection<string> QueuedCommands { get; }
    public IReadOnlyCollection<PaymentCommandExecutionRecord> ExecutionLog { get; }
    public PaymentCommandState FinalState { get; }
    public string Explanation { get; }
}
