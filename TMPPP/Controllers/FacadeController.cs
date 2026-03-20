using TMPPP.Domain.Structural.Facade;
using TMPPP.Views;

namespace TMPPP.Controllers;

public class FacadeController
{
    private readonly MainMenuView _view;
    private readonly PaymentCheckoutFacade _paymentCheckoutFacade;

    public FacadeController(MainMenuView view, PaymentCheckoutFacade paymentCheckoutFacade)
    {
        _view = view;
        _paymentCheckoutFacade = paymentCheckoutFacade;
    }

    public void RunCheckoutDemo()
    {
        var request = new CheckoutRequest(
            "Demo Buyer",
            "buyer@demo.local",
            199.99m,
            "RON",
            "card",
            DateTime.UtcNow.AddDays(7));

        var response = _paymentCheckoutFacade.ExecuteCheckout(request);
        _view.ShowFacadeDemo(request, response);
    }
}
