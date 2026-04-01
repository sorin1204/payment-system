namespace TMPPP.Domain.Behavioral.Command;

public sealed class CapturePaymentCommand : PaymentCommandBase
{
    public CapturePaymentCommand(PaymentCommandContext context)
        : base(context)
    {
    }

    public override string Name => "CapturePayment";
    public override string Description => "Confirms settlement and captures the authorized amount.";

    protected override void Apply(PaymentCommandContext context)
    {
        context.Capture();
    }
}
