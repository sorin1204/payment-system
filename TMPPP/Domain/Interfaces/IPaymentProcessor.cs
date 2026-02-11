using TMPPP.Domain.Entities;

namespace TMPPP.Domain.Interfaces;

public interface IPaymentProcessor
{
    PaymentResult Process(Payment payment, IPaymentMethod method);
}
