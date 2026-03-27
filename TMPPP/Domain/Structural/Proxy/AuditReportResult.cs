namespace TMPPP.Domain.Structural.Proxy;

public sealed class AuditReportResult
{
    public AuditReportResult(
        string requestedBy,
        string role,
        bool accessGranted,
        bool realServiceInitialized,
        IReadOnlyCollection<AuditReportEntry> entries,
        string message)
    {
        RequestedBy = requestedBy;
        Role = role;
        AccessGranted = accessGranted;
        RealServiceInitialized = realServiceInitialized;
        Entries = entries;
        Message = message;
    }

    public string RequestedBy { get; }
    public string Role { get; }
    public bool AccessGranted { get; }
    public bool RealServiceInitialized { get; }
    public IReadOnlyCollection<AuditReportEntry> Entries { get; }
    public string Message { get; }
}
