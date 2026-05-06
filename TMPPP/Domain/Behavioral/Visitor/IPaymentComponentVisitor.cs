using TMPPP.Domain.Structural.Composite;

namespace TMPPP.Domain.Behavioral.Visitor;

public interface IPaymentComponentVisitor
{
    void VisitBatch(PaymentBatch batch, int depth);
    void VisitLeaf(PaymentLeaf leaf, int depth);
}
