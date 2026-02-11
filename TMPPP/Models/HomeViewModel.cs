using TMPPP.Domain.Entities;

namespace TMPPP.Models;

public sealed class HomeViewModel
{
    public string? Message { get; set; }
    public List<Invoice> Invoices { get; set; } = new();
    public List<Payment> Payments { get; set; } = new();
}
