using TMPPP.Domain.Entities;
using TMPPP.Domain.Interfaces;

namespace TMPPP.Infrastructure.InMemory;

public sealed class InMemoryPaymentRepository : IPaymentRepository
{
    private readonly Dictionary<Guid, Payment> _items = new();

    public Payment? GetById(Guid id)
    {
        return _items.TryGetValue(id, out var payment) ? payment : null;
    }

    public void Add(Payment payment)
    {
        _items[payment.Id] = payment;
    }

    public void Update(Payment payment)
    {
        _items[payment.Id] = payment;
    }
}
