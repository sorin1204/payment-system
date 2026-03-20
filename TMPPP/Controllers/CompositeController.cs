using TMPPP.Domain.Structural.Composite;
using TMPPP.Views;

namespace TMPPP.Controllers;

public class CompositeController
{
    private readonly MainMenuView _view;

    public CompositeController(MainMenuView view)
    {
        _view = view;
    }

    public void RunPaymentBatchDemo()
    {
        var batch = BuildPaymentBatch();
        _view.ShowCompositeDemo(batch);
    }

    public static PaymentBatch BuildPaymentBatch()
    {
        var cardPayments = new PaymentBatch("Card payments", "RON")
            .Add(new PaymentLeaf("Order #1001", 120m, "RON"))
            .Add(new PaymentLeaf("Order #1002", 85.50m, "RON"));

        var walletPayments = new PaymentBatch("Wallet payments", "RON")
            .Add(new PaymentLeaf("Google Pay order #1003", 64.99m, "RON"))
            .Add(new PaymentLeaf("PayPal order #1004", 210m, "RON"));

        var failedRetryBatch = new PaymentBatch("Retry batch", "RON")
            .Add(new PaymentLeaf("Retry order #0998", 49.99m, "RON"))
            .Add(new PaymentLeaf("Retry order #0999", 130m, "RON"));

        return new PaymentBatch("Daily settlement", "RON")
            .Add(cardPayments)
            .Add(walletPayments)
            .Add(failedRetryBatch)
            .Add(new PaymentLeaf("Manual bank transfer", 300m, "RON"));
    }
}
