using TMPPP.Domain.Entities;
using TMPPP.Domain.Interfaces;

namespace TMPPP.Infrastructure.InMemory;

public sealed class InMemoryPaymentRepository : IPaymentRepository
{
    private readonly Dictionary<Guid, Payment> _payments = new();

    public Payment? GetById(Guid id)
    {
        _payments.TryGetValue(id, out var payment);
        return payment;
    }

    public void Add(Payment payment)
    {
        _payments[payment.Id] = payment;
    }

    public void Update(Payment payment)
    {
        _payments[payment.Id] = payment;
    }

    public IReadOnlyCollection<Payment> GetAll()
    {
        return _payments.Values.ToList();
    }
}
