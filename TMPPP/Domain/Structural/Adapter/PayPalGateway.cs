namespace TMPPP.Domain.Structural.Adapter;

public class PayPalGateway
{
    public string SendPayment(decimal amount, string currencyCode)
    {
        return $"PAYPAL-{currencyCode}-{amount:0.00}-{Guid.NewGuid():N}";
    }
}
