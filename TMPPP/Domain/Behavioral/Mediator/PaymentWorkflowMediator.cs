namespace TMPPP.Domain.Behavioral.Mediator;

public sealed class PaymentWorkflowMediator : IPaymentWorkflowMediator
{
    private readonly decimal _fraudReviewThreshold;
    private readonly PaymentGatewayDesk _gatewayDesk;
    private readonly FraudReviewDesk _fraudReviewDesk;
    private readonly AccountingDesk _accountingDesk;
    private readonly CustomerNotificationDesk _notificationDesk;

    public PaymentWorkflowMediator(
        PaymentGatewayDesk gatewayDesk,
        FraudReviewDesk fraudReviewDesk,
        AccountingDesk accountingDesk,
        CustomerNotificationDesk notificationDesk,
        decimal fraudReviewThreshold = 1000m)
    {
        _gatewayDesk = gatewayDesk;
        _fraudReviewDesk = fraudReviewDesk;
        _accountingDesk = accountingDesk;
        _notificationDesk = notificationDesk;
        _fraudReviewThreshold = fraudReviewThreshold;

        _gatewayDesk.SetMediator(this);
        _fraudReviewDesk.SetMediator(this);
        _accountingDesk.SetMediator(this);
        _notificationDesk.SetMediator(this);
    }

    public PaymentMediatorDemoResult Start(PaymentCoordinationContext context)
    {
        _gatewayDesk.Submit(context);

        return new PaymentMediatorDemoResult(
            context.PaymentReference,
            context.Amount,
            context.Currency,
            context.Method,
            context.CurrentStatus,
            context.FraudReviewTriggered,
            context.CustomerMessage,
            context.Timeline.ToList(),
            new[]
            {
                _gatewayDesk.Snapshot(),
                _fraudReviewDesk.Snapshot(),
                _accountingDesk.Snapshot(),
                _notificationDesk.Snapshot()
            },
            "Participantii nu comunica direct intre ei. Payment gateway, fraud review, accounting si customer notification trimit semnale doar mediatorului, iar acesta decide cine trebuie activat mai departe.");
    }

    public void Notify(PaymentParticipant sender, string eventName, PaymentCoordinationContext context)
    {
        switch (sender)
        {
            case PaymentGatewayDesk when eventName == "payment-submitted":
                if (RequiresFraudReview(context))
                {
                    _gatewayDesk.MoveToReview(context);
                    _fraudReviewDesk.Review(context);
                }
                else
                {
                    _accountingDesk.Record(context);
                }

                break;

            case FraudReviewDesk when eventName == "fraud-approved":
                _gatewayDesk.MarkApproved(context);
                _accountingDesk.Record(context);
                break;

            case FraudReviewDesk when eventName == "fraud-rejected":
                _gatewayDesk.MarkRejected(context);
                _notificationDesk.Send(
                    context,
                    "Clientul a fost informat ca plata a fost oprita dupa verificarea de risc.");
                break;

            case AccountingDesk when eventName == "accounting-recorded":
                _gatewayDesk.MarkBooked(context);
                _notificationDesk.Send(
                    context,
                    $"Clientul a primit confirmarea pentru plata {context.PaymentReference} de {context.Amount} {context.Currency}.");
                break;

            case CustomerNotificationDesk when eventName == "customer-notified":
                context.AddTimeline("Mediator", sender.Name, "workflow-complete", "Mediatorul a inchis coordonarea dupa notificarea clientului.");
                break;
        }
    }

    private bool RequiresFraudReview(PaymentCoordinationContext context)
    {
        return context.Amount >= _fraudReviewThreshold || context.Method == "bank";
    }
}
