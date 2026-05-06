using TMPPP.Domain.Entities;
using TMPPP.Domain.Interfaces;

namespace TMPPP.Domain.Behavioral.Chain;

public sealed class LoadPaymentHandler : PaymentHandlerBase
{
    private readonly IPaymentRepository _paymentRepository;

    public LoadPaymentHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public override PaymentResult Handle(PaymentChainContext context)
    {
        var payment = _paymentRepository.GetById(context.PaymentId);
        if (payment is null)
        {
            return Fail(context, "payment_not_found", $"Plata {context.PaymentId} nu exista.");
        }

        context.Payment = payment;
        Pass(context, $"Plata {payment.Id} a fost incarcata pentru procesare.");
        return Continue(context);
    }
}
