using TMPPP.Domain.Entities;
using TMPPP.Domain.Interfaces;

namespace TMPPP.Infrastructure.InMemory;

public sealed class InMemoryInvoiceRepository : IInvoiceRepository
{
    private readonly Dictionary<Guid, Invoice> _invoices = new();

    public Invoice? GetById(Guid id)
    {
        _invoices.TryGetValue(id, out var invoice);
        return invoice;
    }

    public void Add(Invoice invoice)
    {
        _invoices[invoice.Id] = invoice;
    }

    public void Update(Invoice invoice)
    {
        _invoices[invoice.Id] = invoice;
    }

    public IReadOnlyCollection<Invoice> GetAll()
    {
        return _invoices.Values.ToList();
    }
}
