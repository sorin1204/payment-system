namespace TMPPP.Domain.Behavioral.Observer;

public sealed class ObserverNotificationJournal
{
    private readonly List<ObserverNotificationEntry> _entries = [];

    public IReadOnlyCollection<ObserverNotificationEntry> Entries => _entries;

    public void Add(string observer, string destination, string message)
    {
        _entries.Add(new ObserverNotificationEntry(observer, destination, message));
    }
}
