namespace TMPPP.Domain.Behavioral.Iterator;

public sealed class IteratorTraversalStep
{
    public IteratorTraversalStep(int position, PaymentQueueItem item)
    {
        Position = position;
        Item = item;
    }

    public int Position { get; }
    public PaymentQueueItem Item { get; }
}
