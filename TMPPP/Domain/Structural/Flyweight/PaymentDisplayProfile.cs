namespace TMPPP.Domain.Structural.Flyweight;

public sealed class PaymentDisplayProfile
{
    public PaymentDisplayProfile(
        string paymentMethod,
        string currency,
        string status,
        string processingChannel,
        string receiptFooter)
    {
        PaymentMethod = paymentMethod;
        Currency = currency;
        Status = status;
        ProcessingChannel = processingChannel;
        ReceiptFooter = receiptFooter;
    }

    public string PaymentMethod { get; }
    public string Currency { get; }
    public string Status { get; }
    public string ProcessingChannel { get; }
    public string ReceiptFooter { get; }

    public string Describe() =>
        $"{PaymentMethod.ToUpperInvariant()} | {Currency.ToUpperInvariant()} | {Status} | {ProcessingChannel}";
}
