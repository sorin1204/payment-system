namespace TMPPP.Domain.Structural.Adapter;

public class StripeGateway
{
    public bool MakeCharge(int amountInCents, string currency, out string chargeId)
    {
        chargeId = $"STRIPE-{currency}-{amountInCents}-{Guid.NewGuid():N}";
        return amountInCents > 0;
    }
}
