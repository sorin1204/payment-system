using TMPPP.Domain.Entities;

namespace TMPPP.Domain.Interfaces;

public interface IInvoiceRepository
{
    Invoice? GetById(Guid id);
    void Add(Invoice invoice);
    void Update(Invoice invoice);
}
