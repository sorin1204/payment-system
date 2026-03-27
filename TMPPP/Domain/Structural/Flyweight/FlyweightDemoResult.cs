namespace TMPPP.Domain.Structural.Flyweight;

public sealed class FlyweightDemoResult
{
    public FlyweightDemoResult(
        IReadOnlyCollection<PaymentDocumentEntry> entries,
        int uniqueFlyweights,
        int savedObjects,
        bool cardProfileShared,
        string explanation)
    {
        Entries = entries;
        UniqueFlyweights = uniqueFlyweights;
        SavedObjects = savedObjects;
        CardProfileShared = cardProfileShared;
        Explanation = explanation;
    }

    public IReadOnlyCollection<PaymentDocumentEntry> Entries { get; }
    public int TotalEntries => Entries.Count;
    public int UniqueFlyweights { get; }
    public int SavedObjects { get; }
    public bool CardProfileShared { get; }
    public string Explanation { get; }
}
