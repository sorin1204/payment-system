namespace TMPPP.Domain.Behavioral.Command;

public sealed class PaymentCommandContext
{
    private readonly List<string> _auditTrail = [];

    public PaymentCommandContext(string paymentReference, decimal amount, string currency)
    {
        PaymentReference = paymentReference;
        Amount = amount;
        Currency = currency;
        Status = "Pending";
    }

    public string PaymentReference { get; }
    public decimal Amount { get; }
    public string Currency { get; }
    public bool Authorized { get; private set; }
    public bool Captured { get; private set; }
    public bool Refunded { get; private set; }
    public string Status { get; private set; }
    public IReadOnlyCollection<string> AuditTrail => _auditTrail;

    public PaymentCommandState CreateSnapshot()
    {
        return new PaymentCommandState(Authorized, Captured, Refunded, Status, _auditTrail.ToList());
    }

    public void Restore(PaymentCommandState snapshot)
    {
        Authorized = snapshot.Authorized;
        Captured = snapshot.Captured;
        Refunded = snapshot.Refunded;
        Status = snapshot.Status;

        _auditTrail.Clear();
        _auditTrail.AddRange(snapshot.AuditTrail);
    }

    public void Authorize()
    {
        if (Authorized)
        {
            throw new InvalidOperationException("Payment is already authorized.");
        }

        Authorized = true;
        Status = "Authorized";
        _auditTrail.Add($"Authorized {Amount:0.00} {Currency}.");
    }

    public void Capture()
    {
        if (!Authorized)
        {
            throw new InvalidOperationException("Payment must be authorized before capture.");
        }

        if (Captured)
        {
            throw new InvalidOperationException("Payment is already captured.");
        }

        Captured = true;
        Refunded = false;
        Status = "Captured";
        _auditTrail.Add($"Captured {Amount:0.00} {Currency}.");
    }

    public void Refund()
    {
        if (!Captured)
        {
            throw new InvalidOperationException("Payment must be captured before refund.");
        }

        if (Refunded)
        {
            throw new InvalidOperationException("Payment is already refunded.");
        }

        Refunded = true;
        Status = "Refunded";
        _auditTrail.Add($"Refunded {Amount:0.00} {Currency}.");
    }
}
