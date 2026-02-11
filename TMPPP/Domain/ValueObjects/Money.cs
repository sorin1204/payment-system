namespace TMPPP.Domain.ValueObjects;

public sealed class Money
{
    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    private Money()
    {
        Currency = string.Empty;
    }

    public decimal Amount { get; }
    public string Currency { get; }

    public static Money Zero(string currency)
    {
        return new Money(0m, currency);
    }
}
