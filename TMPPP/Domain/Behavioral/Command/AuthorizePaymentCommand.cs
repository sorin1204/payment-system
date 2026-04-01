namespace TMPPP.Domain.Behavioral.Command;

public sealed class AuthorizePaymentCommand : PaymentCommandBase
{
    public AuthorizePaymentCommand(PaymentCommandContext context)
        : base(context)
    {
    }

    public override string Name => "AuthorizePayment";
    public override string Description => "Reserves the amount on the customer's payment method.";

    protected override void Apply(PaymentCommandContext context)
    {
        context.Authorize();
    }
}
