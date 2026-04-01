using TMPPP.Domain.Behavioral.Command;

namespace TMPPP.Controllers;

public class CommandController
{
    public static CommandDemoResult BuildPaymentCommandDemo(
        IReadOnlyCollection<string> operations,
        int undoSteps,
        int redoSteps,
        decimal amount,
        string currency)
    {
        var paymentReference = $"PAY-{Guid.NewGuid():N}"[..12].ToUpperInvariant();
        var context = new PaymentCommandContext(paymentReference, amount, currency);
        var invoker = new PaymentCommandInvoker();
        var queuedCommands = new List<string>();

        foreach (var operation in operations)
        {
            var command = CreateCommand(operation, context);
            invoker.Enqueue(command);
            queuedCommands.Add(command.Name);
        }

        invoker.ExecuteAll(() => context.Status);
        invoker.UndoLast(undoSteps, () => context.Status);
        invoker.RedoLast(redoSteps, () => context.Status);

        return new CommandDemoResult(
            paymentReference,
            amount,
            currency,
            queuedCommands,
            invoker.Records.ToList(),
            context.CreateSnapshot(),
            "Invoker-ul stocheaza comenzile separat de executia lor, apoi ruleaza, anuleaza si reface operatiile fara sa cunoasca detaliile concrete ale receiver-ului.");
    }

    private static IPaymentCommand CreateCommand(string operation, PaymentCommandContext context)
    {
        return operation.Trim().ToLowerInvariant() switch
        {
            "authorize" => new AuthorizePaymentCommand(context),
            "capture" => new CapturePaymentCommand(context),
            "refund" => new RefundPaymentCommand(context),
            _ => throw new ArgumentException("Operations must be one of: authorize, capture, refund.")
        };
    }
}
