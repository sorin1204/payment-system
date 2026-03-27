using TMPPP.Domain.Interfaces;
using TMPPP.Domain.Structural.Decorator;
using TMPPP.Infrastructure.Notifications;
using TMPPP.Views;

namespace TMPPP.Controllers;

public class DecoratorController
{
    private readonly MainMenuView _view;

    public DecoratorController(MainMenuView view)
    {
        _view = view;
    }

    public void RunNotificationDecoratorDemo()
    {
        var result = BuildNotificationDecoratorDemo();
        _view.ShowDecoratorDemo(result);
    }

    public static DecoratorDemoResult BuildNotificationDecoratorDemo()
    {
        var context = new DecoratorNotificationContext();
        INotificationService service = new ConsoleNotificationService();
        service = new EmailNotificationDecorator(service, context);
        service = new SmsNotificationDecorator(service, context);
        service = new PushNotificationDecorator(service, context);

        const string recipient = "client@tmppp.local";
        const string subject = "Payment processed";
        const string message = "Invoice #INV-2026-031 has been paid successfully.";

        service.Notify(recipient, subject, message);

        return new DecoratorDemoResult(
            recipient,
            subject,
            message,
            context.DeliveredChannels,
            "Decorator inveleste serviciul de notificare de baza si adauga canale noi dinamic, fara sa modifice implementarea initiala.");
    }
}
