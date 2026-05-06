using TMPPP.Domain.Entities;
using TMPPP.Domain.Enums;

namespace TMPPP.Domain.Behavioral.State;

public sealed class FailedPaymentState : PaymentStateBase
{
    private static readonly IReadOnlyCollection<string> Actions = Array.Empty<string>();

    public override PaymentStatus Status => PaymentStatus.Failed;
    public override string Name => "Failed";
    public override IReadOnlyCollection<string> AllowedActions => Actions;

    public override PaymentStateTransitionResult Handle(Payment payment, PaymentStateAction action)
    {
        return Reject(payment, action, "Starea Failed este terminala in acest flux si nu mai accepta tranzitii.");
    }
}
