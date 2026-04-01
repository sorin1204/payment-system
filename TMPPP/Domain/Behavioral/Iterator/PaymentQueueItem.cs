namespace TMPPP.Domain.Behavioral.Iterator;

public sealed class PaymentQueueItem
{
    public PaymentQueueItem(string reference, string customerName, decimal amount, string currency, string method, string status)
    {
        Reference = reference;
        CustomerName = customerName;
        Amount = amount;
        Currency = currency;
        Method = method;
        Status = status;
    }

    public string Reference { get; }
    public string CustomerName { get; }
    public decimal Amount { get; }
    public string Currency { get; }
    public string Method { get; }
    public string Status { get; }
}
