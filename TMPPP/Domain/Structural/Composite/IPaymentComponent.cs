namespace TMPPP.Domain.Structural.Composite;

public interface IPaymentComponent
{
    string Name { get; }
    decimal GetAmount();
    string Render(int depth = 0);
}
