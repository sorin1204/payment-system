namespace TMPPP.Domain.Behavioral.Iterator;

public sealed class PaymentBatchIterator : IPaymentQueueIterator
{
    private readonly IReadOnlyList<PaymentQueueItem> _items;
    private int _index;

    public PaymentBatchIterator(IReadOnlyList<PaymentQueueItem> items)
    {
        _items = items;
        _index = 0;
    }

    public PaymentQueueItem First()
    {
        if (_items.Count == 0)
        {
            throw new InvalidOperationException("Collection is empty.");
        }

        _index = 0;
        return _items[_index];
    }

    public PaymentQueueItem Next()
    {
        if (!HasMore())
        {
            throw new InvalidOperationException("No more items in the collection.");
        }

        var item = _items[_index];
        _index++;
        return item;
    }

    public PaymentQueueItem Current()
    {
        if (_items.Count == 0)
        {
            throw new InvalidOperationException("Collection is empty.");
        }

        var currentIndex = _index == 0 ? 0 : Math.Min(_index - 1, _items.Count - 1);
        return _items[currentIndex];
    }

    public bool HasMore()
    {
        return _index < _items.Count;
    }
}
