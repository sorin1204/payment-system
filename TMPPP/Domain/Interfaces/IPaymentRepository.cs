using TMPPP.Domain.Entities;

namespace TMPPP.Domain.Interfaces;

public interface IPaymentRepository
{
    Payment? GetById(Guid id);
    void Add(Payment payment);
    void Update(Payment payment);
}
