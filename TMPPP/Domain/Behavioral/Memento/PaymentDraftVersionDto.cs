namespace TMPPP.Domain.Behavioral.Memento;

public sealed class PaymentDraftVersionDto
{
    public PaymentDraftVersionDto(
        string versionLabel,
        decimal amount,
        string currency,
        string paymentMethod,
        string description,
        DateTime savedAtUtc)
    {
        VersionLabel = versionLabel;
        Amount = amount;
        Currency = currency;
        PaymentMethod = paymentMethod;
        Description = description;
        SavedAtUtc = savedAtUtc;
    }

    public string VersionLabel { get; }
    public decimal Amount { get; }
    public string Currency { get; }
    public string PaymentMethod { get; }
    public string Description { get; }
    public DateTime SavedAtUtc { get; }
}
