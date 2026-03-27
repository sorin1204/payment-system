namespace TMPPP.Domain.Structural.Decorator;

public sealed class DecoratorDemoResult
{
    public DecoratorDemoResult(
        string recipient,
        string subject,
        string message,
        IReadOnlyCollection<NotificationChannel> deliveredChannels,
        string explanation)
    {
        Recipient = recipient;
        Subject = subject;
        Message = message;
        DeliveredChannels = deliveredChannels;
        Explanation = explanation;
    }

    public string Recipient { get; }
    public string Subject { get; }
    public string Message { get; }
    public IReadOnlyCollection<NotificationChannel> DeliveredChannels { get; }
    public string Explanation { get; }
}
