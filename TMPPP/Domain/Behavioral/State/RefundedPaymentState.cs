using TMPPP.Domain.Entities;
using TMPPP.Domain.Enums;

namespace TMPPP.Domain.Behavioral.State;

public sealed class RefundedPaymentState : PaymentStateBase
{
    private static readonly IReadOnlyCollection<string> Actions = Array.Empty<string>();

    public override PaymentStatus Status => PaymentStatus.Refunded;
    public override string Name => "Refunded";
    public override IReadOnlyCollection<string> AllowedActions => Actions;

    public override PaymentStateTransitionResult Handle(Payment payment, PaymentStateAction action)
    {
        return Reject(payment, action, "O plata Refunded este finalizata si nu mai poate schimba starea.");
    }
}
