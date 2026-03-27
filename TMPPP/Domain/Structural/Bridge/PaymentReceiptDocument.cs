namespace TMPPP.Domain.Structural.Bridge;

public sealed class PaymentReceiptDocument : FinanceDocument
{
    private readonly string _paymentReference;
    private readonly string _paymentMethod;
    private readonly decimal _amount;
    private readonly string _currency;
    private readonly DateTime _processedAtUtc;

    public PaymentReceiptDocument(
        string paymentReference,
        string paymentMethod,
        decimal amount,
        string currency,
        DateTime processedAtUtc,
        IDocumentRenderer renderer)
        : base(renderer)
    {
        _paymentReference = paymentReference;
        _paymentMethod = paymentMethod;
        _amount = amount;
        _currency = currency;
        _processedAtUtc = processedAtUtc;
    }

    protected override string GetTitle() => $"Receipt {_paymentReference}";

    protected override IReadOnlyCollection<string> BuildLines() =>
        new[]
        {
            $"Method: {_paymentMethod}",
            $"Paid: {_amount:0.00} {_currency}",
            $"Processed at: {_processedAtUtc:yyyy-MM-dd HH:mm} UTC"
        };

    protected override string GetFooter() => "Payment confirmed successfully.";
}
