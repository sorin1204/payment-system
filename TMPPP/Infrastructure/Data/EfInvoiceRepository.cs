using Microsoft.EntityFrameworkCore;
using TMPPP.Domain.Entities;
using TMPPP.Domain.Interfaces;

namespace TMPPP.Infrastructure.Data;

public sealed class EfInvoiceRepository : IInvoiceRepository
{
    private readonly AppDbContext _db;

    public EfInvoiceRepository(AppDbContext db)
    {
        _db = db;
    }

    public Invoice? GetById(Guid id)
    {
        return _db.Invoices.AsNoTracking().FirstOrDefault(x => x.Id == id);
    }

    public void Add(Invoice invoice)
    {
        _db.Invoices.Add(invoice);
        _db.SaveChanges();
    }

    public void Update(Invoice invoice)
    {
        _db.Invoices.Update(invoice);
        _db.SaveChanges();
    }
}
