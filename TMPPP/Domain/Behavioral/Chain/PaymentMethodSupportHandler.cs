using TMPPP.Domain.Entities;

namespace TMPPP.Domain.Behavioral.Chain;

public sealed class PaymentMethodSupportHandler : PaymentHandlerBase
{
    public override PaymentResult Handle(PaymentChainContext context)
    {
        if (context.Payment is null)
        {
            return Fail(context, "payment_context_missing", "Contextul nu contine plata curenta.");
        }

        var supportsAmount = context.Method.Supports(context.Payment.Amount);
        if (!supportsAmount)
        {
            Pass(context, $"{context.Method.MethodName} nu suporta suma {context.Payment.Amount.Amount} {context.Payment.Amount.Currency}; cererea merge mai departe catre executorul final, care decide rezultatul.");
            return Continue(context);
        }

        Pass(context, $"{context.Method.MethodName} suporta suma si lasa procesarea sa continue.");
        return Continue(context);
    }
}
