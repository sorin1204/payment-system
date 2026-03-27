namespace TMPPP.Domain.Structural.Proxy;

public sealed class ProxyDemoResult
{
    public ProxyDemoResult(
        AuditReportResult deniedAttempt,
        AuditReportResult grantedAttempt,
        string explanation)
    {
        DeniedAttempt = deniedAttempt;
        GrantedAttempt = grantedAttempt;
        Explanation = explanation;
    }

    public AuditReportResult DeniedAttempt { get; }
    public AuditReportResult GrantedAttempt { get; }
    public string Explanation { get; }
}
