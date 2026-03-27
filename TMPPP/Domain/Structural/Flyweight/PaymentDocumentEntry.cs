namespace TMPPP.Domain.Structural.Flyweight;

public sealed class PaymentDocumentEntry
{
    public PaymentDocumentEntry(
        string paymentReference,
        string customerName,
        decimal amount,
        DateTime createdAtUtc,
        PaymentDisplayProfile profile)
    {
        PaymentReference = paymentReference;
        CustomerName = customerName;
        Amount = amount;
        CreatedAtUtc = createdAtUtc;
        Profile = profile;
    }

    public string PaymentReference { get; }
    public string CustomerName { get; }
    public decimal Amount { get; }
    public DateTime CreatedAtUtc { get; }
    public PaymentDisplayProfile Profile { get; }

    public string Render() =>
        $"{PaymentReference} | {CustomerName} | {Amount:0.00} {Profile.Currency} | {Profile.Describe()} | {CreatedAtUtc:yyyy-MM-dd}";
}
