namespace TMPPP.Domain.Structural.Composite;

public class PaymentBatch : IPaymentComponent
{
    private readonly List<IPaymentComponent> _children = new();

    public PaymentBatch(string name, string currency)
    {
        Name = name;
        Currency = currency;
    }

    public string Name { get; }
    public string Currency { get; }
    public IReadOnlyCollection<IPaymentComponent> Children => _children;

    public PaymentBatch Add(IPaymentComponent component)
    {
        _children.Add(component);
        return this;
    }

    public decimal GetAmount()
    {
        return _children.Sum(child => child.GetAmount());
    }

    public string Render(int depth = 0)
    {
        var indent = new string(' ', depth * 2);
        var lines = new List<string> { $"{indent}+ {Name}: total {GetAmount():0.00} {Currency}" };
        lines.AddRange(_children.Select(child => child.Render(depth + 1)));
        return string.Join(Environment.NewLine, lines);
    }
}
