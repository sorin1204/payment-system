namespace TMPPP.Domain.Structural.Adapter;

public class GooglePayGateway
{
    public (bool Ok, string Token) ExecuteTransaction(string description, decimal total)
    {
        var token = $"GPAY-{description.ToUpperInvariant().Replace(' ', '-')}-{Guid.NewGuid():N}";
        return (total > 0, token);
    }
}
