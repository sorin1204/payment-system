using TMPPP.Domain.Entities;

namespace TMPPP.Domain.Behavioral.Observer;

public sealed class MerchantDashboardObserver : IPaymentStatusObserver
{
    private readonly ObserverNotificationJournal _journal;

    public MerchantDashboardObserver(ObserverNotificationJournal journal)
    {
        _journal = journal;
    }

    public string Name => "Merchant dashboard";

    public void Update(Payment payment, PaymentStatusChangedEvent statusChangedEvent)
    {
        _journal.Add(
            Name,
            "merchant-dashboard",
            $"Invoice {payment.InvoiceId} now has payment status {statusChangedEvent.CurrentStatus} for {statusChangedEvent.Amount:0.00} {statusChangedEvent.Currency}.");
    }
}
