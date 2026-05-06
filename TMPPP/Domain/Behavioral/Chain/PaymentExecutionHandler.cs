using TMPPP.Domain.Entities;
using TMPPP.Domain.Interfaces;

namespace TMPPP.Domain.Behavioral.Chain;

public sealed class PaymentExecutionHandler : PaymentHandlerBase
{
    private readonly IPaymentProcessor _paymentProcessor;

    public PaymentExecutionHandler(IPaymentProcessor paymentProcessor)
    {
        _paymentProcessor = paymentProcessor;
    }

    public override PaymentResult Handle(PaymentChainContext context)
    {
        if (context.Payment is null)
        {
            return Fail(context, "payment_context_missing", "Contextul nu contine plata curenta.");
        }

        var result = _paymentProcessor.Process(context.Payment, context.Method);
        context.AddStep(
            GetType().Name,
            result.Success ? "handled" : "stopped",
            $"Executia finala a returnat: {result.Message}");

        return new PaymentResult(result.Success, result.Message, result.FailureCode, context.Steps.ToList());
    }
}
