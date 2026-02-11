using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using TMPPP.Domain.PaymentMethods;
using TMPPP.Domain.Services;
using TMPPP.Domain.ValueObjects;
using TMPPP.Infrastructure.InMemory;
using TMPPP.Models;

namespace TMPPP.Controllers;

public sealed class HomeController : Controller
{
    private readonly IPaymentService _paymentService;
    private readonly InMemoryInvoiceRepository _invoiceRepository;
    private readonly InMemoryPaymentRepository _paymentRepository;

    public HomeController(
        IPaymentService paymentService,
        InMemoryInvoiceRepository invoiceRepository,
        InMemoryPaymentRepository paymentRepository)
    {
        _paymentService = paymentService;
        _invoiceRepository = invoiceRepository;
        _paymentRepository = paymentRepository;
    }

    [HttpGet]
    public IActionResult Index(string? message)
    {
        var model = new HomeViewModel
        {
            Message = message,
            Invoices = _invoiceRepository.GetAll()
                .OrderByDescending(x => x.DueDate)
                .ToList(),
            Payments = _paymentRepository.GetAll()
                .OrderByDescending(x => x.CreatedAt)
                .ToList()
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult CreateInvoice(string? customerId, string? total, string? currency)
    {
        var customerGuid = ParseGuidOrNew(customerId);
        var amount = ParseDecimal(total);
        var currencyCode = ParseStringOrDefault(currency, "RON");

        if (amount <= 0m)
        {
            return RedirectToAction(nameof(Index), new { message = "Total must be positive." });
        }

        var invoice = new Domain.Entities.Invoice(
            Guid.NewGuid(),
            customerGuid,
            new Money(amount, currencyCode),
            DateTime.UtcNow.AddDays(14));
        _invoiceRepository.Add(invoice);

        return RedirectToAction(nameof(Index), new { message = $"Invoice created: {invoice.Id}" });
    }

    [HttpPost]
    public IActionResult CreatePayment(string? invoiceId, string? amount, string? currency)
    {
        var invoiceGuid = ParseGuid(invoiceId);
        var total = ParseDecimal(amount);
        var currencyCode = ParseStringOrDefault(currency, "RON");

        if (invoiceGuid == Guid.Empty)
        {
            return RedirectToAction(nameof(Index), new { message = "Invalid invoice id." });
        }

        if (total <= 0m)
        {
            return RedirectToAction(nameof(Index), new { message = "Amount must be positive." });
        }

        try
        {
            var payment = _paymentService.CreatePayment(invoiceGuid, new Money(total, currencyCode));
            return RedirectToAction(nameof(Index), new { message = $"Payment created: {payment.Id}" });
        }
        catch (Exception ex)
        {
            return RedirectToAction(nameof(Index), new { message = $"Error: {ex.Message}" });
        }
    }

    [HttpPost]
    public IActionResult ProcessPayment(string? paymentId, string? method)
    {
        var paymentGuid = ParseGuid(paymentId);
        var methodKey = ParseStringOrDefault(method, "cash");

        if (paymentGuid == Guid.Empty)
        {
            return RedirectToAction(nameof(Index), new { message = "Invalid payment id." });
        }

        Domain.Interfaces.IPaymentMethod paymentMethod = methodKey switch
        {
            "card" => new CardPaymentMethod("Demo User", "4242"),
            "bank" => new BankTransferPaymentMethod("RO00BANK0000000000", "DemoBank"),
            _ => new CashPaymentMethod()
        };

        try
        {
            var result = _paymentService.ProcessPayment(paymentGuid, paymentMethod);
            var status = result.Success ? "OK" : "Failed";
            return RedirectToAction(nameof(Index), new { message = $"Process result: {status}. {result.Message}" });
        }
        catch (Exception ex)
        {
            return RedirectToAction(nameof(Index), new { message = $"Error: {ex.Message}" });
        }
    }

    private static Guid ParseGuid(string? value)
    {
        return Guid.TryParse(value, out var id) ? id : Guid.Empty;
    }

    private static Guid ParseGuidOrNew(string? value)
    {
        return Guid.TryParse(value, out var id) ? id : Guid.NewGuid();
    }

    private static decimal ParseDecimal(string? value)
    {
        if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var amount))
        {
            return amount;
        }

        if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out amount))
        {
            return amount;
        }

        return 0m;
    }

    private static string ParseStringOrDefault(string? value, string defaultValue)
    {
        return string.IsNullOrWhiteSpace(value) ? defaultValue : value.Trim();
    }
}
