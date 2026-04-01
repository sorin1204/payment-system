namespace TMPPP.Domain.Behavioral.Command;

public sealed class PaymentCommandInvoker
{
    private readonly Queue<IPaymentCommand> _queue = new();
    private readonly Stack<IPaymentCommand> _history = new();
    private readonly Stack<IPaymentCommand> _redoStack = new();
    private readonly List<PaymentCommandExecutionRecord> _records = [];

    public IReadOnlyCollection<PaymentCommandExecutionRecord> Records => _records;

    public void Enqueue(IPaymentCommand command)
    {
        _queue.Enqueue(command);
    }

    public void ExecuteAll(Func<string> currentStatus)
    {
        while (_queue.Count > 0)
        {
            var command = _queue.Dequeue();
            command.Execute();
            _history.Push(command);
            _redoStack.Clear();
            _records.Add(new PaymentCommandExecutionRecord("execute", command.Name, currentStatus(), command.Description));
        }
    }

    public void UndoLast(int steps, Func<string> currentStatus)
    {
        for (var i = 0; i < steps && _history.Count > 0; i++)
        {
            var command = _history.Pop();
            command.Undo();
            _redoStack.Push(command);
            _records.Add(new PaymentCommandExecutionRecord("undo", command.Name, currentStatus(), $"Undo {command.Description.ToLowerInvariant()}"));
        }
    }

    public void RedoLast(int steps, Func<string> currentStatus)
    {
        for (var i = 0; i < steps && _redoStack.Count > 0; i++)
        {
            var command = _redoStack.Pop();
            command.Execute();
            _history.Push(command);
            _records.Add(new PaymentCommandExecutionRecord("redo", command.Name, currentStatus(), $"Redo {command.Description.ToLowerInvariant()}"));
        }
    }
}
