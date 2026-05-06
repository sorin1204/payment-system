using TMPPP.Domain.Behavioral.Visitor;

namespace TMPPP.Domain.Structural.Composite;

public class PaymentLeaf : IPaymentComponent
{
    public PaymentLeaf(string name, decimal amount, string currency)
    {
        Name = name;
        Amount = amount;
        Currency = currency;
    }

    public string Name { get; }
    public decimal Amount { get; }
    public string Currency { get; }

    public decimal GetAmount()
    {
        return Amount;
    }

    public string Render(int depth = 0)
    {
        var indent = new string(' ', depth * 2);
        return $"{indent}- {Name}: {Amount:0.00} {Currency}";
    }

    public void Accept(IPaymentComponentVisitor visitor, int depth = 0)
    {
        visitor.VisitLeaf(this, depth);
    }
}
