namespace TMPPP.Domain.Structural.Adapter;

public class PayPalAdapter : IOnlinePaymentGateway
{
    private readonly PayPalGateway _payPalGateway;

    public PayPalAdapter(PayPalGateway payPalGateway)
    {
        _payPalGateway = payPalGateway;
    }

    public PaymentResponse Pay(PaymentRequest request)
    {
        var transactionId = _payPalGateway.SendPayment(request.Amount, request.Currency);
        return new PaymentResponse(true, "PayPal", transactionId, "Payment processed through PayPal adapter.");
    }
}
