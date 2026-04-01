using TMPPP.Domain.Entities;
using TMPPP.Domain.Enums;

namespace TMPPP.Domain.Behavioral.Observer;

public sealed class AccountingAlertObserver : IPaymentStatusObserver
{
    private readonly ObserverNotificationJournal _journal;

    public AccountingAlertObserver(ObserverNotificationJournal journal)
    {
        _journal = journal;
    }

    public string Name => "Accounting alerts";

    public void Update(Payment payment, PaymentStatusChangedEvent statusChangedEvent)
    {
        var message = statusChangedEvent.CurrentStatus switch
        {
            PaymentStatus.Processed => "Accounting can register settlement for the payment.",
            PaymentStatus.Failed => "Accounting keeps the invoice open because the payment failed.",
            PaymentStatus.Refunded => "Accounting must register the refund and reconcile the ledger.",
            _ => "Accounting received a payment status update."
        };

        _journal.Add(Name, "accounting-stream", message);
    }
}
