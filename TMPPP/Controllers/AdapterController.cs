using TMPPP.Domain.Structural.Adapter;
using TMPPP.Views;

namespace TMPPP.Controllers;

public class AdapterController
{
    private readonly MainMenuView _view;

    public AdapterController(MainMenuView view)
    {
        _view = view;
    }

    public void RunPaymentGatewayDemo()
    {
        var request = new PaymentRequest(249.99m, "RON", "Laborator structural patterns");
        var gateways = new List<IOnlinePaymentGateway>
        {
            new PayPalAdapter(new PayPalGateway()),
            new StripeAdapter(new StripeGateway()),
            new GooglePayAdapter(new GooglePayGateway())
        };

        var responses = gateways.Select(gateway => gateway.Pay(request)).ToList();
        _view.ShowAdapterDemo(request, responses);
    }
}
