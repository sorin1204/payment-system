using TMPPP.Domain.Entities;

namespace TMPPP.Domain.Interfaces;

public interface IRefundablePaymentMethod : IPaymentMethod
{
    PaymentResult Refund(Payment payment);
}
