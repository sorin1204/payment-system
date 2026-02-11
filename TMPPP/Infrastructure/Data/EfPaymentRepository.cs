using Microsoft.EntityFrameworkCore;
using TMPPP.Domain.Entities;
using TMPPP.Domain.Interfaces;

namespace TMPPP.Infrastructure.Data;

public sealed class EfPaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _db;

    public EfPaymentRepository(AppDbContext db)
    {
        _db = db;
    }

    public Payment? GetById(Guid id)
    {
        return _db.Payments.AsNoTracking().FirstOrDefault(x => x.Id == id);
    }

    public void Add(Payment payment)
    {
        _db.Payments.Add(payment);
        _db.SaveChanges();
    }

    public void Update(Payment payment)
    {
        _db.Payments.Update(payment);
        _db.SaveChanges();
    }
}
