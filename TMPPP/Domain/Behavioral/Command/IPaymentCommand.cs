namespace TMPPP.Domain.Behavioral.Command;

public interface IPaymentCommand
{
    string Name { get; }
    string Description { get; }
    void Execute();
    void Undo();
}
