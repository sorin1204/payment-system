using TMPPP.Domain.Interfaces;

namespace TMPPP.Domain.Structural.Decorator;

public abstract class NotificationServiceDecorator : INotificationService
{
    private readonly INotificationService _inner;
    private readonly DecoratorNotificationContext _context;

    protected NotificationServiceDecorator(INotificationService inner, DecoratorNotificationContext context)
    {
        _inner = inner;
        _context = context;
    }

    protected DecoratorNotificationContext Context => _context;

    public virtual void Notify(string recipient, string subject, string message)
    {
        _inner.Notify(recipient, subject, message);
    }
}
