using TMPPP.Domain.Entities;

namespace TMPPP.Domain.Behavioral.TemplateMethod;

public sealed class BankTransferPaymentTemplate : PaymentProcessingTemplate
{
    protected override string MethodKey => "bank";

    protected override string PrepareChannel(Payment payment)
    {
        return "Se genereaza instructiunile de transfer si referinta bancara pentru plata.";
    }

    protected override string CollectFunds(Payment payment)
    {
        return "Se asteapta confirmarea bancii si reconcilierea transferului in contul comerciantului.";
    }

    protected override string ConfirmSettlement(Payment payment)
    {
        return "Se valideaza extrasul bancar si se confirma incasarea prin transfer.";
    }

    protected override string? AfterSettlement(Payment payment)
    {
        return "Hook-ul specific transferului bancar notifica departamentul financiar pentru arhivarea documentelor suport.";
    }
}
