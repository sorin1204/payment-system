namespace TMPPP.Domain.Behavioral.Iterator;

public sealed class IteratorDemoResult
{
    public IteratorDemoResult(
        int totalPayments,
        PaymentQueueItem firstPayment,
        PaymentQueueItem currentPayment,
        IReadOnlyCollection<IteratorTraversalStep> traversal,
        string explanation)
    {
        TotalPayments = totalPayments;
        FirstPayment = firstPayment;
        CurrentPayment = currentPayment;
        Traversal = traversal;
        Explanation = explanation;
    }

    public int TotalPayments { get; }
    public PaymentQueueItem FirstPayment { get; }
    public PaymentQueueItem CurrentPayment { get; }
    public IReadOnlyCollection<IteratorTraversalStep> Traversal { get; }
    public string Explanation { get; }
}
