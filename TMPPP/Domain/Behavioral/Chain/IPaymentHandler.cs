using TMPPP.Domain.Entities;

namespace TMPPP.Domain.Behavioral.Chain;

public interface IPaymentHandler
{
    IPaymentHandler SetNext(IPaymentHandler next);
    PaymentResult Handle(PaymentChainContext context);
}
