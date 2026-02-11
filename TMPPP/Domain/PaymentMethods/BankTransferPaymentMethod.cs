using TMPPP.Domain.Entities;
using TMPPP.Domain.ValueObjects;

namespace TMPPP.Domain.PaymentMethods;

public sealed class BankTransferPaymentMethod : PaymentMethodBase
{
    public BankTransferPaymentMethod(string iban, string bankName)
        : base("BankTransfer")
    {
        Iban = iban;
        BankName = bankName;
    }

    public string Iban { get; }
    public string BankName { get; }

    public override bool Supports(Money amount)
    {
        return amount.Amount > 0m;
    }

    public override PaymentResult Process(Payment payment)
    {
        return new PaymentResult(true, "Bank transfer initiated.");
    }
}
