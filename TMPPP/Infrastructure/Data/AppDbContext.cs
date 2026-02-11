using Microsoft.EntityFrameworkCore;
using TMPPP.Domain.Entities;

namespace TMPPP.Infrastructure.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().HasKey(x => x.Id);
        modelBuilder.Entity<Invoice>().HasKey(x => x.Id);
        modelBuilder.Entity<Payment>().HasKey(x => x.Id);

        modelBuilder.Entity<Invoice>().OwnsOne(x => x.Total);
        modelBuilder.Entity<Payment>().OwnsOne(x => x.Amount);
    }
}
