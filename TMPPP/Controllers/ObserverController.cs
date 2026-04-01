using TMPPP.Domain.Behavioral.Observer;
using TMPPP.Domain.Entities;
using TMPPP.Domain.Enums;
using TMPPP.Domain.ValueObjects;

namespace TMPPP.Controllers;

public class ObserverController
{
    public static ObserverDemoResult BuildPaymentObserverDemo(PaymentStatus targetStatus, decimal amount, string currency)
    {
        var journal = new ObserverNotificationJournal();
        var subject = new PaymentStatusSubject("Payment status notifier");

        subject.Attach(new CustomerMobileAppObserver(journal));
        subject.Attach(new MerchantDashboardObserver(journal));
        subject.Attach(new AccountingAlertObserver(journal));

        var payment = new Payment(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new Money(amount, currency),
            DateTime.UtcNow);

        var statusChangedEvent = subject.ChangeStatus(payment, targetStatus, "observer-demo");

        return new ObserverDemoResult(
            subject.Name,
            statusChangedEvent.PaymentReference,
            statusChangedEvent.PreviousStatus,
            statusChangedEvent.CurrentStatus,
            subject.Observers.Select(observer => observer.Name).ToList(),
            journal.Entries.ToList(),
            "Subiectul publica schimbarea starii platii o singura data, iar fiecare observator reactioneaza independent: aplicatia clientului, dashboard-ul comerciantului si contabilitatea.");
    }
}
