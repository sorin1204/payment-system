using TMPPP.Domain.Interfaces;
using TMPPP.Domain.PaymentMethods;

namespace TMPPP.Domain.Factories;

public sealed class CardPaymentMethodCreator : PaymentMethodCreator
{
    public override IPaymentMethod CreatePaymentMethod()
    {
        return new CardPaymentMethod("Demo User", "4242");
    }
}
