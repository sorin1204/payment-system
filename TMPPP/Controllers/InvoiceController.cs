using TMPPP.Domain.Entities;
using TMPPP.Domain.Interfaces;
using TMPPP.Domain.ValueObjects;
using TMPPP.Views;

namespace TMPPP.Controllers;

public class InvoiceController
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly MainMenuView _view;

    public InvoiceController(IInvoiceRepository invoiceRepository, MainMenuView view)
    {
        _invoiceRepository = invoiceRepository;
        _view = view;
    }

    public void CreateInvoice()
    {
        var customerInput = _view.Prompt("Customer id (blank for random): ");
        var customerId = string.IsNullOrWhiteSpace(customerInput)
            ? Guid.NewGuid()
            : Guid.TryParse(customerInput, out var parsedId)
                ? parsedId
                : Guid.NewGuid();

        var amountInput = _view.Prompt("Total amount: ", "0");
        if (!decimal.TryParse(amountInput, out var amount))
        {
            _view.ShowMessage("Invalid amount.");
            return;
        }

        var currency = _view.Prompt("Currency (e.g., RON): ", "RON");
        var invoice = new Invoice(Guid.NewGuid(), customerId, new Money(amount, currency), DateTime.UtcNow.AddDays(14));

        _invoiceRepository.Add(invoice);
        _view.ShowInvoiceCreated(invoice);
    }

    public void ListInvoices()
    {
        var invoices = _invoiceRepository.GetAll()
            .OrderByDescending(x => x.DueDate)
            .ToList();

        _view.ShowInvoices(invoices);
    }
}
