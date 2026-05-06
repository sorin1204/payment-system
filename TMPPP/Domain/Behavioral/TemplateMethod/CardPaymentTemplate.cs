using TMPPP.Domain.Entities;

namespace TMPPP.Domain.Behavioral.TemplateMethod;

public sealed class CardPaymentTemplate : PaymentProcessingTemplate
{
    protected override string MethodKey => "card";

    protected override string PrepareChannel(Payment payment)
    {
        return "Se initializeaza canalul card si tokenizarea datelor sensibile ale clientului.";
    }

    protected override string CollectFunds(Payment payment)
    {
        return "Se ruleaza autorizarea instant si se rezerva suma pe card.";
    }

    protected override string ConfirmSettlement(Payment payment)
    {
        return "Se confirma captura cardului si se emite confirmarea catre comerciant.";
    }

    protected override string? AfterSettlement(Payment payment)
    {
        return "Hook-ul specific cardului trimite chitanta digitala si marcheaza tranzactia pentru reconciliere rapida.";
    }
}
