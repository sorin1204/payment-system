using TMPPP.Domain.Entities;

namespace TMPPP.Domain.Behavioral.Chain;

public abstract class PaymentHandlerBase : IPaymentHandler
{
    private IPaymentHandler? _next;

    public IPaymentHandler SetNext(IPaymentHandler next)
    {
        _next = next;
        return next;
    }

    public abstract PaymentResult Handle(PaymentChainContext context);

    protected PaymentResult Continue(PaymentChainContext context)
    {
        if (_next is null)
        {
            return new PaymentResult(true, "Cererea a parcurs intregul lant de procesare.", chainTrace: context.Steps.ToList());
        }

        return _next.Handle(context);
    }

    protected PaymentResult Fail(PaymentChainContext context, string failureCode, string message)
    {
        context.AddStep(GetType().Name, "stopped", message);
        return new PaymentResult(false, message, failureCode, context.Steps.ToList());
    }

    protected void Pass(PaymentChainContext context, string message)
    {
        context.AddStep(GetType().Name, "passed", message);
    }
}
