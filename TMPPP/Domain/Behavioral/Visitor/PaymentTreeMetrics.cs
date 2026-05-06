namespace TMPPP.Domain.Behavioral.Visitor;

public sealed record PaymentTreeMetrics(
    int BatchCount,
    int LeafCount,
    int MaxDepth,
    decimal TotalAmount,
    string Currency);
