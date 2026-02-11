using TMPPP.Domain.Entities;
using TMPPP.Domain.ValueObjects;

namespace TMPPP.Domain.PaymentMethods;

public sealed class CardPaymentMethod : PaymentMethodBase
{
    public CardPaymentMethod(string cardHolder, string last4)
        : base("Card")
    {
        CardHolder = cardHolder;
        Last4 = last4;
    }

    public string CardHolder { get; }
    public string Last4 { get; }

    public override bool Supports(Money amount)
    {
        return amount.Amount > 0m;
    }

    public override PaymentResult Process(Payment payment)
    {
        return new PaymentResult(true, "Card payment approved.");
    }
}
