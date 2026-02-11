namespace TMPPP.Domain.Interfaces;

public interface INotificationService
{
    void Notify(string recipient, string subject, string message);
}
