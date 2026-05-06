using TMPPP.Domain.Entities;

namespace TMPPP.Domain.Behavioral.TemplateMethod;

public sealed class CashPaymentTemplate : PaymentProcessingTemplate
{
    protected override string MethodKey => "cash";

    protected override string PrepareChannel(Payment payment)
    {
        return "Se deschide sesiunea de incasare la ghiseu si se pregateste registrul de numerar.";
    }

    protected override string CollectFunds(Payment payment)
    {
        return "Casierul incaseaza numerarul si verifica suma primita fata de totalul datorat.";
    }

    protected override string ConfirmSettlement(Payment payment)
    {
        return "Se inchide bonul fiscal si se marcheaza plata cash ca incasata pe loc.";
    }

    protected override string BuildOutcome(Payment payment)
    {
        return $"cash workflow completed instantly for {payment.Amount.Amount} {payment.Amount.Currency}.";
    }
}
