namespace TMPPP.Domain.Behavioral.Memento;

public sealed class PaymentDraftHistory
{
    private readonly List<PaymentDraftSnapshot> _snapshots = [];

    public IReadOnlyCollection<PaymentDraftSnapshot> Snapshots => _snapshots;

    public void Add(PaymentDraftSnapshot snapshot)
    {
        _snapshots.Add(snapshot);
    }

    public PaymentDraftSnapshot Get(string versionLabel)
    {
        var snapshot = _snapshots.FirstOrDefault(x =>
            string.Equals(x.VersionLabel, versionLabel, StringComparison.OrdinalIgnoreCase));

        return snapshot ?? throw new ArgumentException($"Version '{versionLabel}' was not found.");
    }
}
