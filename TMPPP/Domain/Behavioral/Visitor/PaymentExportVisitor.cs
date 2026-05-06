using TMPPP.Domain.Structural.Composite;

namespace TMPPP.Domain.Behavioral.Visitor;

public sealed class PaymentExportVisitor : IPaymentComponentVisitor
{
    private readonly List<string> _batchPath = [];
    private readonly List<PaymentFlattenedEntry> _entries = [];

    public IReadOnlyCollection<PaymentFlattenedEntry> Entries => _entries;

    public void VisitBatch(PaymentBatch batch, int depth)
    {
        if (_batchPath.Count > depth)
        {
            _batchPath.RemoveRange(depth, _batchPath.Count - depth);
        }

        if (_batchPath.Count == depth)
        {
            _batchPath.Add(batch.Name);
        }
        else
        {
            _batchPath[depth] = batch.Name;
        }
    }

    public void VisitLeaf(PaymentLeaf leaf, int depth)
    {
        var activePath = _batchPath.Take(depth).Append(leaf.Name);
        _entries.Add(new PaymentFlattenedEntry(string.Join(" > ", activePath), leaf.Amount, leaf.Currency));
    }
}
