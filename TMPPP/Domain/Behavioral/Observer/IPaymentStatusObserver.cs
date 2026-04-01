using TMPPP.Domain.Entities;

namespace TMPPP.Domain.Behavioral.Observer;

public interface IPaymentStatusObserver
{
    string Name { get; }
    void Update(Payment payment, PaymentStatusChangedEvent statusChangedEvent);
}
