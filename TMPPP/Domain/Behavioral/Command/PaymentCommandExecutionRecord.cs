namespace TMPPP.Domain.Behavioral.Command;

public sealed class PaymentCommandExecutionRecord
{
    public PaymentCommandExecutionRecord(string action, string commandName, string statusAfterAction, string detail)
    {
        Action = action;
        CommandName = commandName;
        StatusAfterAction = statusAfterAction;
        Detail = detail;
    }

    public string Action { get; }
    public string CommandName { get; }
    public string StatusAfterAction { get; }
    public string Detail { get; }
}
