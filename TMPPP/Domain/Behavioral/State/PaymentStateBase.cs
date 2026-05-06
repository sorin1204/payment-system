using TMPPP.Domain.Entities;
using TMPPP.Domain.Enums;

namespace TMPPP.Domain.Behavioral.State;

public abstract class PaymentStateBase : IPaymentState
{
    public abstract PaymentStatus Status { get; }
    public abstract string Name { get; }
    public abstract IReadOnlyCollection<string> AllowedActions { get; }

    public abstract PaymentStateTransitionResult Handle(Payment payment, PaymentStateAction action);

    protected PaymentStateTransitionResult Transition(
        Payment payment,
        PaymentStateAction action,
        IPaymentState nextState,
        string message)
    {
        var previousStatus = payment.Status;
        payment.SetState(nextState);
        return new PaymentStateTransitionResult(
            true,
            action.ToString(),
            previousStatus,
            payment.Status,
            message,
            previousStatus != payment.Status);
    }

    protected PaymentStateTransitionResult Reject(Payment payment, PaymentStateAction action, string message)
    {
        return new PaymentStateTransitionResult(
            false,
            action.ToString(),
            payment.Status,
            payment.Status,
            message,
            false);
    }
}
