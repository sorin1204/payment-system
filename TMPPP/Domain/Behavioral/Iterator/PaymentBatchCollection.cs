namespace TMPPP.Domain.Behavioral.Iterator;

public sealed class PaymentBatchCollection
{
    private readonly List<PaymentQueueItem> _items = [];

    public PaymentBatchCollection(IEnumerable<PaymentQueueItem> items)
    {
        _items.AddRange(items);
    }

    public int Count => _items.Count;

    public IPaymentQueueIterator CreateIterator()
    {
        return new PaymentBatchIterator(_items);
    }
}
