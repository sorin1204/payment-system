using TMPPP.Domain.Entities;
using TMPPP.Domain.Interfaces;
using TMPPP.Domain.ValueObjects;

namespace TMPPP.Domain.PaymentMethods;

public sealed class CashPaymentMethod : PaymentMethodBase, IRefundablePaymentMethod
{
    public CashPaymentMethod()
        : base("Cash")
    {
    }

    public override bool Supports(Money amount)
    {
        return amount.Amount > 0m;
    }

    public override PaymentResult Process(Payment payment)
    {
        return new PaymentResult(true, "Cash received.");
    }

    public PaymentResult Refund(Payment payment)
    {
        return new PaymentResult(true, "Cash refunded.");
    }
}
