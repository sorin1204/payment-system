using TMPPP.Domain.Entities;
using TMPPP.Domain.Enums;

namespace TMPPP.Domain.Behavioral.State;

public sealed class ProcessedPaymentState : PaymentStateBase
{
    private static readonly IReadOnlyCollection<string> Actions = ["refund-requested"];

    public override PaymentStatus Status => PaymentStatus.Processed;
    public override string Name => "Processed";
    public override IReadOnlyCollection<string> AllowedActions => Actions;

    public override PaymentStateTransitionResult Handle(Payment payment, PaymentStateAction action)
    {
        return action switch
        {
            PaymentStateAction.RefundRequested => Transition(
                payment,
                action,
                new RefundedPaymentState(),
                "Plata procesata a fost returnata si trece in Refunded."),
            PaymentStateAction.ProcessSucceeded => Reject(
                payment,
                action,
                "O plata deja Processed nu mai poate fi procesata inca o data."),
            PaymentStateAction.ProcessFailed => Reject(
                payment,
                action,
                "O plata Processed nu poate deveni Failed prin aceeasi executie."),
            _ => Reject(payment, action, "Actiune neacceptata pentru starea Processed.")
        };
    }
}
