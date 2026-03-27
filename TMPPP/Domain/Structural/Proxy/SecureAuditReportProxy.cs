namespace TMPPP.Domain.Structural.Proxy;

public sealed class SecureAuditReportProxy : IAuditReportService
{
    private readonly AuditUserContext _userContext;
    private readonly Lazy<IAuditReportService> _realService;

    public SecureAuditReportProxy(AuditUserContext userContext)
    {
        _userContext = userContext;
        _realService = new Lazy<IAuditReportService>(() => new RealAuditReportService());
    }

    public AuditReportResult GenerateMonthlyAudit()
    {
        if (!HasAccess(_userContext.Role))
        {
            return new AuditReportResult(
                _userContext.UserName,
                _userContext.Role,
                false,
                _realService.IsValueCreated,
                Array.Empty<AuditReportEntry>(),
                "Acces interzis. Doar rolurile admin sau finance-analyst pot vedea raportul financiar.");
        }

        var result = _realService.Value.GenerateMonthlyAudit();
        return new AuditReportResult(
            _userContext.UserName,
            _userContext.Role,
            true,
            _realService.IsValueCreated,
            result.Entries,
            "Proxy-ul a permis accesul si a initializat serviciul real doar dupa validarea rolului.");
    }

    private static bool HasAccess(string role)
    {
        return role.Trim().ToLowerInvariant() is "admin" or "finance-analyst";
    }
}
