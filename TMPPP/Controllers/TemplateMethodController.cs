using TMPPP.Domain.Behavioral.TemplateMethod;
using TMPPP.Domain.Entities;
using TMPPP.Domain.ValueObjects;

namespace TMPPP.Controllers;

public static class TemplateMethodController
{
    public static PaymentTemplateDemoResult BuildPaymentTemplateMethodDemo(decimal amount, string currency, string method)
    {
        var template = ResolveTemplate(method);
        var payment = new Payment(Guid.NewGuid(), Guid.NewGuid(), new Money(amount, currency), DateTime.UtcNow);
        return template.Execute(payment);
    }

    private static PaymentProcessingTemplate ResolveTemplate(string? method)
    {
        return method?.Trim().ToLowerInvariant() switch
        {
            "card" => new CardPaymentTemplate(),
            "bank" or "banktransfer" => new BankTransferPaymentTemplate(),
            "cash" => new CashPaymentTemplate(),
            _ => throw new ArgumentException("Method must be one of: card, bank, cash.")
        };
    }
}
