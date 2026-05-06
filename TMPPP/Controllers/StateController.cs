using TMPPP.Domain.Behavioral.State;
using TMPPP.Domain.Entities;
using TMPPP.Domain.Enums;
using TMPPP.Domain.ValueObjects;

namespace TMPPP.Controllers;

public static class StateController
{
    public static object BuildPaymentStateDemo(
        PaymentStatus initialStatus,
        decimal amount,
        string currency,
        IReadOnlyCollection<string> actions)
    {
        var payment = new Payment(Guid.NewGuid(), Guid.NewGuid(), new Money(amount, currency), DateTime.UtcNow);
        SeedInitialState(payment, initialStatus);

        var transitions = new List<PaymentStateTransitionResult>();
        foreach (var action in actions)
        {
            transitions.Add(ApplyAction(payment, action));
        }

        return new
        {
            paymentId = payment.Id,
            initialStatus = initialStatus.ToString(),
            currentStatus = payment.Status.ToString(),
            currentState = payment.StateName,
            availableActions = payment.GetAvailableActions(),
            transitions,
            explanation =
                "Obiectul Payment isi delega comportamentul catre clasa de stare curenta. Fiecare stare decide singura ce actiuni accepta si catre ce stare urmatoare se face tranzitia."
        };
    }

    private static PaymentStateTransitionResult ApplyAction(Payment payment, string action)
    {
        return action.Trim().ToLowerInvariant() switch
        {
            "process-succeeded" => payment.MarkProcessed(),
            "process-failed" => payment.MarkFailed(),
            "refund-requested" => payment.MarkRefunded(),
            _ => throw new ArgumentException("Actions must be one of: process-succeeded, process-failed, refund-requested.")
        };
    }

    private static void SeedInitialState(Payment payment, PaymentStatus initialStatus)
    {
        switch (initialStatus)
        {
            case PaymentStatus.Pending:
                return;
            case PaymentStatus.Processed:
                payment.ForceState(PaymentStateFactory.Create(PaymentStatus.Processed));
                return;
            case PaymentStatus.Failed:
                payment.ForceState(PaymentStateFactory.Create(PaymentStatus.Failed));
                return;
            case PaymentStatus.Refunded:
                payment.ForceState(PaymentStateFactory.Create(PaymentStatus.Refunded));
                return;
            default:
                throw new ArgumentOutOfRangeException(nameof(initialStatus), initialStatus, "Unsupported payment state.");
        }
    }
}
