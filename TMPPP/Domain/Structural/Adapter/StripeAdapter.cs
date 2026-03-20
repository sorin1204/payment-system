namespace TMPPP.Domain.Structural.Adapter;

public class StripeAdapter : IOnlinePaymentGateway
{
    private readonly StripeGateway _stripeGateway;

    public StripeAdapter(StripeGateway stripeGateway)
    {
        _stripeGateway = stripeGateway;
    }

    public PaymentResponse Pay(PaymentRequest request)
    {
        var amountInCents = (int)Math.Round(request.Amount * 100m, MidpointRounding.AwayFromZero);
        var success = _stripeGateway.MakeCharge(amountInCents, request.Currency.ToLowerInvariant(), out var chargeId);
        return new PaymentResponse(
            success,
            "Stripe",
            chargeId,
            success ? "Payment processed through Stripe adapter." : "Stripe rejected the payment.");
    }
}
