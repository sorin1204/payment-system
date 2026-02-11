using TMPPP.Domain.Interfaces;

namespace TMPPP.Infrastructure.Notifications;

public sealed class ConsoleNotificationService : INotificationService
{
    public void Notify(string recipient, string subject, string message)
    {
        Console.WriteLine($"[Notify] To: {recipient} | {subject} | {message}");
    }
}
