using TMPPP.Domain.Entities;

namespace TMPPP.Domain.Behavioral.TemplateMethod;

public abstract class PaymentProcessingTemplate
{
    public PaymentTemplateDemoResult Execute(Payment payment)
    {
        var steps = new List<PaymentTemplateStep>();
        var paymentReference = $"TMP-{payment.Id:N}"[..12].ToUpperInvariant();

        Record(steps, "validate", ValidatePayment(payment));
        Record(steps, "prepare", PrepareChannel(payment));
        Record(steps, "collect", CollectFunds(payment));
        Record(steps, "confirm", ConfirmSettlement(payment));

        var hookMessage = AfterSettlement(payment);
        if (!string.IsNullOrWhiteSpace(hookMessage))
        {
            Record(steps, "after-settlement", hookMessage);
        }

        return new PaymentTemplateDemoResult(
            GetType().Name,
            paymentReference,
            payment.Amount.Amount,
            payment.Amount.Currency,
            MethodKey,
            steps,
            BuildOutcome(payment),
            "Metoda sablon din clasa de baza fixeaza ordinea algoritmului: validare, pregatire, colectare, confirmare si hook optional. Clasele derivate personalizeaza doar pasii specifici fiecarei metode de plata.");
    }

    protected abstract string MethodKey { get; }

    protected virtual string ValidatePayment(Payment payment)
    {
        if (payment.Amount.Amount <= 0)
        {
            throw new ArgumentException("Payment amount must be greater than 0.");
        }

        return $"Plata {payment.Id} a fost validata pentru suma {payment.Amount.Amount} {payment.Amount.Currency}.";
    }

    protected abstract string PrepareChannel(Payment payment);
    protected abstract string CollectFunds(Payment payment);
    protected abstract string ConfirmSettlement(Payment payment);

    protected virtual string? AfterSettlement(Payment payment)
    {
        return null;
    }

    protected virtual string BuildOutcome(Payment payment)
    {
        return $"{MethodKey} workflow completed for {payment.Amount.Amount} {payment.Amount.Currency}.";
    }

    private static void Record(ICollection<PaymentTemplateStep> steps, string step, string detail)
    {
        steps.Add(new PaymentTemplateStep(step, detail));
    }
}
