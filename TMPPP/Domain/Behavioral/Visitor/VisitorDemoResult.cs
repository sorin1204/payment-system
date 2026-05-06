namespace TMPPP.Domain.Behavioral.Visitor;

public sealed record VisitorDemoResult(
    string RootBatch,
    PaymentTreeMetrics Metrics,
    IReadOnlyCollection<PaymentFlattenedEntry> ExportedPayments,
    string Explanation);
