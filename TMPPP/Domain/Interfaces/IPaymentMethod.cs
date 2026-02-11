using TMPPP.Domain.Entities;
using TMPPP.Domain.ValueObjects;

namespace TMPPP.Domain.Interfaces;

public interface IPaymentMethod
{
    string MethodName { get; }
    bool Supports(Money amount);
    PaymentResult Process(Payment payment);
}
