using TMPPP.Domain.Interfaces;

namespace TMPPP.Domain.Factories;

public abstract class PaymentMethodCreator
{
    public abstract IPaymentMethod CreatePaymentMethod();
}
