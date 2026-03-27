using TMPPP.Domain.Structural.Bridge;
using TMPPP.Views;

namespace TMPPP.Controllers;

public class BridgeController
{
    private readonly MainMenuView _view;

    public BridgeController(MainMenuView view)
    {
        _view = view;
    }

    public void RunBridgeDemo()
    {
        var result = BuildBridgeDemo();
        _view.ShowBridgeDemo(result);
    }

    public static BridgeDemoResult BuildBridgeDemo()
    {
        var renderers = new IDocumentRenderer[]
        {
            new MobileRenderer(),
            new EmailRenderer(),
            new KioskRenderer()
        };

        var items = new List<BridgeDemoItem>();

        foreach (var renderer in renderers)
        {
            var invoice = new InvoiceDocument(
                "INV-2026-045",
                "Demo Buyer",
                349.99m,
                "RON",
                new DateTime(2026, 4, 5, 0, 0, 0, DateTimeKind.Utc),
                renderer);

            var receipt = new PaymentReceiptDocument(
                "PAY-2026-112",
                "Card",
                349.99m,
                "RON",
                new DateTime(2026, 3, 27, 14, 30, 0, DateTimeKind.Utc),
                renderer);

            items.Add(new BridgeDemoItem("Invoice", invoice.RendererName, invoice.Render()));
            items.Add(new BridgeDemoItem("Payment receipt", receipt.RendererName, receipt.Render()));
        }

        return new BridgeDemoResult(
            items,
            "Bridge separa tipul documentului financiar de canalul de redare, astfel incat poti adauga documente sau renderere noi fara a multiplica clasele pentru fiecare combinatie.");
    }
}
