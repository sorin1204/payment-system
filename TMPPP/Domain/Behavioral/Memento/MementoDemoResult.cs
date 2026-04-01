namespace TMPPP.Domain.Behavioral.Memento;

public sealed class MementoDemoResult
{
    public MementoDemoResult(
        PaymentDraftVersionDto currentDraft,
        string restoredVersion,
        IReadOnlyCollection<PaymentDraftVersionDto> savedVersions,
        string explanation)
    {
        CurrentDraft = currentDraft;
        RestoredVersion = restoredVersion;
        SavedVersions = savedVersions;
        Explanation = explanation;
    }

    public PaymentDraftVersionDto CurrentDraft { get; }
    public string RestoredVersion { get; }
    public IReadOnlyCollection<PaymentDraftVersionDto> SavedVersions { get; }
    public string Explanation { get; }
}
