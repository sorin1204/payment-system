namespace TMPPP.Domain.ValueObjects;

public readonly struct Money
{
    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public decimal Amount { get; }
    public string Currency { get; }

    public static Money Zero(string currency)
    {
        return new Money(0m, currency);
    }
}
