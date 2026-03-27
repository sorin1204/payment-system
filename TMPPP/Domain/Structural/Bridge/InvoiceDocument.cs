namespace TMPPP.Domain.Structural.Bridge;

public sealed class InvoiceDocument : FinanceDocument
{
    private readonly string _invoiceNumber;
    private readonly string _customerName;
    private readonly decimal _amount;
    private readonly string _currency;
    private readonly DateTime _dueDateUtc;

    public InvoiceDocument(
        string invoiceNumber,
        string customerName,
        decimal amount,
        string currency,
        DateTime dueDateUtc,
        IDocumentRenderer renderer)
        : base(renderer)
    {
        _invoiceNumber = invoiceNumber;
        _customerName = customerName;
        _amount = amount;
        _currency = currency;
        _dueDateUtc = dueDateUtc;
    }

    protected override string GetTitle() => $"Invoice {_invoiceNumber}";

    protected override IReadOnlyCollection<string> BuildLines() =>
        new[]
        {
            $"Customer: {_customerName}",
            $"Total: {_amount:0.00} {_currency}",
            $"Due date: {_dueDateUtc:yyyy-MM-dd}"
        };

    protected override string GetFooter() => "Please pay before the due date.";
}
