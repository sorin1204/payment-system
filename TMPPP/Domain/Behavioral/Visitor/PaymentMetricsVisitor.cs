using TMPPP.Domain.Structural.Composite;

namespace TMPPP.Domain.Behavioral.Visitor;

public sealed class PaymentMetricsVisitor : IPaymentComponentVisitor
{
    private int _batchCount;
    private int _leafCount;
    private int _maxDepth;
    private decimal _totalAmount;
    private string _currency = "RON";

    public PaymentTreeMetrics Result => new(
        _batchCount,
        _leafCount,
        _maxDepth,
        _totalAmount,
        _currency);

    public void VisitBatch(PaymentBatch batch, int depth)
    {
        _batchCount++;
        _maxDepth = Math.Max(_maxDepth, depth);
        _currency = batch.Currency;
    }

    public void VisitLeaf(PaymentLeaf leaf, int depth)
    {
        _leafCount++;
        _maxDepth = Math.Max(_maxDepth, depth);
        _totalAmount += leaf.Amount;
        _currency = leaf.Currency;
    }
}
