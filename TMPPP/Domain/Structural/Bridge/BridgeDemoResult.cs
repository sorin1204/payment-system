namespace TMPPP.Domain.Structural.Bridge;

public sealed class BridgeDemoResult
{
    public BridgeDemoResult(IReadOnlyCollection<BridgeDemoItem> items, string explanation)
    {
        Items = items;
        Explanation = explanation;
    }

    public IReadOnlyCollection<BridgeDemoItem> Items { get; }
    public string Explanation { get; }
}
