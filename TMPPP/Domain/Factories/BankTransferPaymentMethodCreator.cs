using TMPPP.Domain.Interfaces;
using TMPPP.Domain.PaymentMethods;

namespace TMPPP.Domain.Factories;

public sealed class BankTransferPaymentMethodCreator : PaymentMethodCreator
{
    public override IPaymentMethod CreatePaymentMethod()
    {
        return new BankTransferPaymentMethod("RO00BANK0000000000", "DemoBank");
    }
}
