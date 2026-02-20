using TMPPP.Domain.Entities;
using TMPPP.Domain.Interfaces;

namespace TMPPP.Infrastructure.InMemory;

public sealed class InMemoryInvoiceRepository : IInvoiceRepository
{
    private readonly Dictionary<Guid, Invoice> _items = new();

    public Invoice? GetById(Guid id)
    {
        return _items.TryGetValue(id, out var invoice) ? invoice : null;
    }

    public void Add(Invoice invoice)
    {
        _items[invoice.Id] = invoice;
    }

    public void Update(Invoice invoice)
    {
        _items[invoice.Id] = invoice;
    }

    public IReadOnlyCollection<Invoice> GetAll()
    {
        return _items.Values.ToList();
    }
}
