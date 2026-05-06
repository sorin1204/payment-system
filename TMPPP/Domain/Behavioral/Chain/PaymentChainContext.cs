using TMPPP.Domain.Entities;
using TMPPP.Domain.Interfaces;

namespace TMPPP.Domain.Behavioral.Chain;

public sealed class PaymentChainContext
{
    public PaymentChainContext(Guid paymentId, IPaymentMethod method)
    {
        PaymentId = paymentId;
        Method = method;
    }

    public Guid PaymentId { get; }
    public IPaymentMethod Method { get; }
    public Payment? Payment { get; set; }
    public Invoice? Invoice { get; set; }
    public List<PaymentChainStep> Steps { get; } = [];

    public void AddStep(string handler, string outcome, string message)
    {
        Steps.Add(new PaymentChainStep(handler, outcome, message));
    }
}
