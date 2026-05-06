using TMPPP.Domain.Entities;
using TMPPP.Domain.Enums;

namespace TMPPP.Domain.Behavioral.State;

public sealed class PendingPaymentState : PaymentStateBase
{
    private static readonly IReadOnlyCollection<string> Actions = ["process-succeeded", "process-failed"];

    public override PaymentStatus Status => PaymentStatus.Pending;
    public override string Name => "Pending";
    public override IReadOnlyCollection<string> AllowedActions => Actions;

    public override PaymentStateTransitionResult Handle(Payment payment, PaymentStateAction action)
    {
        return action switch
        {
            PaymentStateAction.ProcessSucceeded => Transition(
                payment,
                action,
                new ProcessedPaymentState(),
                "Plata a fost autorizata si trece din Pending in Processed."),
            PaymentStateAction.ProcessFailed => Transition(
                payment,
                action,
                new FailedPaymentState(),
                "Procesarea a esuat si plata trece din Pending in Failed."),
            PaymentStateAction.RefundRequested => Reject(
                payment,
                action,
                "O plata Pending nu poate fi rambursata inainte de procesare."),
            _ => Reject(payment, action, "Actiune neacceptata pentru starea Pending.")
        };
    }
}
