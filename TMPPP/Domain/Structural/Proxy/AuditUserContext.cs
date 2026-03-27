namespace TMPPP.Domain.Structural.Proxy;

public sealed class AuditUserContext
{
    public AuditUserContext(string userName, string role)
    {
        UserName = userName;
        Role = role;
    }

    public string UserName { get; }
    public string Role { get; }
}
