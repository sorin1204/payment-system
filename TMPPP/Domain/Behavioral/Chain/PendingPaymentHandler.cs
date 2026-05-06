using TMPPP.Domain.Entities;
using TMPPP.Domain.Enums;

namespace TMPPP.Domain.Behavioral.Chain;

public sealed class PendingPaymentHandler : PaymentHandlerBase
{
    public override PaymentResult Handle(PaymentChainContext context)
    {
        if (context.Payment is null)
        {
            return Fail(context, "payment_context_missing", "Contextul nu contine plata curenta.");
        }

        if (context.Payment.Status != PaymentStatus.Pending)
        {
            return Fail(
                context,
                "payment_not_pending",
                $"Plata este deja in starea {context.Payment.Status} si nu mai poate fi reprocesata.");
        }

        Pass(context, "Plata este in stare Pending si poate continua in lant.");
        return Continue(context);
    }
}
