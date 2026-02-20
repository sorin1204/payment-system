using TMPPP.Domain.Interfaces;
using TMPPP.Domain.PaymentMethods;

namespace TMPPP.Domain.Factories;

public sealed class CashPaymentMethodCreator : PaymentMethodCreator
{
    public override IPaymentMethod CreatePaymentMethod()
    {
        return new CashPaymentMethod();
    }
}
