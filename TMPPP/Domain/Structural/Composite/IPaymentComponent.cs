using TMPPP.Domain.Behavioral.Visitor;

namespace TMPPP.Domain.Structural.Composite;

public interface IPaymentComponent
{
    string Name { get; }
    decimal GetAmount();
    string Render(int depth = 0);
    void Accept(IPaymentComponentVisitor visitor, int depth = 0);
}
