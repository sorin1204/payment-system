namespace TMPPP.Domain.Behavioral.Memento;

public sealed class PaymentDraft
{
    public PaymentDraft(decimal amount, string currency, string paymentMethod, string description)
    {
        Amount = amount;
        Currency = currency;
        PaymentMethod = paymentMethod;
        Description = description;
    }

    public decimal Amount { get; private set; }
    public string Currency { get; private set; }
    public string PaymentMethod { get; private set; }
    public string Description { get; private set; }

    public void UpdateAmount(decimal amount)
    {
        Amount = amount;
    }

    public void UpdateCurrency(string currency)
    {
        Currency = currency;
    }

    public void UpdatePaymentMethod(string paymentMethod)
    {
        PaymentMethod = paymentMethod;
    }

    public void UpdateDescription(string description)
    {
        Description = description;
    }

    public PaymentDraftSnapshot Save(string versionLabel)
    {
        return new PaymentDraftSnapshot(
            versionLabel,
            Amount,
            Currency,
            PaymentMethod,
            Description,
            DateTime.UtcNow);
    }

    public void Restore(PaymentDraftSnapshot snapshot)
    {
        Amount = snapshot.Amount;
        Currency = snapshot.Currency;
        PaymentMethod = snapshot.PaymentMethod;
        Description = snapshot.Description;
    }
}
