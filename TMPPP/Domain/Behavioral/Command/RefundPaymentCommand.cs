namespace TMPPP.Domain.Behavioral.Command;

public sealed class RefundPaymentCommand : PaymentCommandBase
{
    public RefundPaymentCommand(PaymentCommandContext context)
        : base(context)
    {
    }

    public override string Name => "RefundPayment";
    public override string Description => "Returns the captured amount back to the customer.";

    protected override void Apply(PaymentCommandContext context)
    {
        context.Refund();
    }
}
