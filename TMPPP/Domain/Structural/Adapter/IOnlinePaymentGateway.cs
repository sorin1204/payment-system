namespace TMPPP.Domain.Structural.Adapter;

public interface IOnlinePaymentGateway
{
    PaymentResponse Pay(PaymentRequest request);
}
