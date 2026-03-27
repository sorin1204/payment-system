using System.Collections.Concurrent;

namespace TMPPP.Domain.Structural.Flyweight;

public sealed class PaymentDisplayProfileFactory
{
    private readonly ConcurrentDictionary<string, PaymentDisplayProfile> _profiles = new();

    public PaymentDisplayProfile GetOrCreate(
        string paymentMethod,
        string currency,
        string status,
        string processingChannel,
        string receiptFooter)
    {
        var normalizedMethod = paymentMethod.Trim().ToLowerInvariant();
        var normalizedCurrency = currency.Trim().ToUpperInvariant();
        var normalizedStatus = status.Trim().ToLowerInvariant();
        var normalizedChannel = processingChannel.Trim();
        var normalizedFooter = receiptFooter.Trim();
        var key = $"{normalizedMethod}|{normalizedCurrency}|{normalizedStatus}|{normalizedChannel}|{normalizedFooter}";

        return _profiles.GetOrAdd(
            key,
            _ => new PaymentDisplayProfile(
                normalizedMethod,
                normalizedCurrency,
                normalizedStatus,
                normalizedChannel,
                normalizedFooter));
    }

    public int SharedInstanceCount => _profiles.Count;
}
