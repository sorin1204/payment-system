namespace TMPPP.Domain.Structural.Adapter;

public class GooglePayAdapter : IOnlinePaymentGateway
{
    private readonly GooglePayGateway _googlePayGateway;

    public GooglePayAdapter(GooglePayGateway googlePayGateway)
    {
        _googlePayGateway = googlePayGateway;
    }

    public PaymentResponse Pay(PaymentRequest request)
    {
        var result = _googlePayGateway.ExecuteTransaction(request.Description, request.Amount);
        return new PaymentResponse(
            result.Ok,
            "Google Pay",
            result.Token,
            result.Ok ? "Payment processed through Google Pay adapter." : "Google Pay rejected the payment.");
    }
}
