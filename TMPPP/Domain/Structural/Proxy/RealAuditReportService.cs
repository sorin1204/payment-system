namespace TMPPP.Domain.Structural.Proxy;

public sealed class RealAuditReportService : IAuditReportService
{
    public AuditReportResult GenerateMonthlyAudit()
    {
        var entries = new List<AuditReportEntry>
        {
            new("Processed card payments", 14250m, "RON", "Processed"),
            new("Pending bank transfers", 4800m, "RON", "Pending"),
            new("Refunded transactions", 1250m, "RON", "Refunded")
        };

        return new AuditReportResult(
            string.Empty,
            string.Empty,
            true,
            true,
            entries,
            "Raportul real contine date financiare sensibile despre plati si poate fi accesat doar de utilizatori autorizati.");
    }
}
