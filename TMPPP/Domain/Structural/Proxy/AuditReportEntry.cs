namespace TMPPP.Domain.Structural.Proxy;

public sealed class AuditReportEntry
{
    public AuditReportEntry(string label, decimal amount, string currency, string status)
    {
        Label = label;
        Amount = amount;
        Currency = currency;
        Status = status;
    }

    public string Label { get; }
    public decimal Amount { get; }
    public string Currency { get; }
    public string Status { get; }
}
