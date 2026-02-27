using Microsoft.EntityFrameworkCore;
using TMPPP.Domain.Interfaces;
using TMPPP.Infrastructure.Data;
using TMPPP.Infrastructure.Notifications;

namespace TMPPP.Domain.Factories.AbstractFactory;

public sealed class SqlitePaymentDomainFactory : PaymentDomainFactory
{
    private readonly AppDbContext _dbContext;

    public SqlitePaymentDomainFactory(string connectionString)
    {
        var singletonConnection = SqliteConnectionManager.Instance.GetConnection(connectionString);
        var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(singletonConnection)
            .Options;

        _dbContext = new AppDbContext(dbOptions);
        _dbContext.Database.EnsureCreated();
    }

    public override IInvoiceRepository CreateInvoiceRepository()
    {
        return new EfInvoiceRepository(_dbContext);
    }

    public override IPaymentRepository CreatePaymentRepository()
    {
        return new EfPaymentRepository(_dbContext);
    }

    public override INotificationService CreateNotificationService()
    {
        return new ConsoleNotificationService();
    }

    public override void Dispose()
    {
        _dbContext.Dispose();
    }
}
