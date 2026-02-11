using TMPPP.Domain.Entities;
using TMPPP.Domain.Interfaces;
using TMPPP.Domain.ValueObjects;

namespace TMPPP.Domain.PaymentMethods;

public abstract class PaymentMethodBase : IPaymentMethod
{
    protected PaymentMethodBase(string methodName)
    {
        MethodName = methodName;
    }

    public string MethodName { get; }

    public abstract bool Supports(Money amount);
    public abstract PaymentResult Process(Payment payment);
}
