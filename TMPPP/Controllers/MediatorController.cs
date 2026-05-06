using TMPPP.Domain.Behavioral.Mediator;

namespace TMPPP.Controllers;

public static class MediatorController
{
    public static PaymentMediatorDemoResult BuildPaymentMediatorDemo(
        decimal amount,
        string currency,
        string method,
        PaymentFraudDecision fraudDecision)
    {
        var normalizedMethod = method.Trim().ToLowerInvariant();
        if (normalizedMethod is not ("card" or "bank" or "cash"))
        {
            throw new ArgumentException("Method must be one of: card, bank, cash.");
        }

        var context = new PaymentCoordinationContext(
            $"MED-{Guid.NewGuid():N}"[..12].ToUpperInvariant(),
            amount,
            currency,
            normalizedMethod,
            fraudDecision);

        var mediator = new PaymentWorkflowMediator(
            new PaymentGatewayDesk(),
            new FraudReviewDesk(),
            new AccountingDesk(),
            new CustomerNotificationDesk());

        return mediator.Start(context);
    }
}
