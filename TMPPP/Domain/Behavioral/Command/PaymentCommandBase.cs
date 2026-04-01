namespace TMPPP.Domain.Behavioral.Command;

public abstract class PaymentCommandBase : IPaymentCommand
{
    private readonly PaymentCommandContext _context;
    private PaymentCommandState? _snapshot;

    protected PaymentCommandBase(PaymentCommandContext context)
    {
        _context = context;
    }

    public abstract string Name { get; }
    public abstract string Description { get; }

    public void Execute()
    {
        _snapshot = _context.CreateSnapshot();
        Apply(_context);
    }

    public void Undo()
    {
        if (_snapshot is null)
        {
            throw new InvalidOperationException("Command has not been executed yet.");
        }

        _context.Restore(_snapshot);
    }

    protected abstract void Apply(PaymentCommandContext context);
}
