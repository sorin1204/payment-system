using TMPPP.Views;

namespace TMPPP.Controllers;

public class AppController
{
    private readonly AdapterController _adapterController;
    private readonly CompositeController _compositeController;
    private readonly FacadeController _facadeController;
    private readonly InvoiceController _invoiceController;
    private readonly PaymentController _paymentController;
    private readonly BurgerController _burgerController;
    private readonly PrototypeController _prototypeController;
    private readonly SingletonController _singletonController;
    private readonly FlyweightController _flyweightController;
    private readonly DecoratorController _decoratorController;
    private readonly BridgeController _bridgeController;
    private readonly ProxyController _proxyController;
    private readonly MainMenuView _view;

    public AppController(
        AdapterController adapterController,
        CompositeController compositeController,
        FacadeController facadeController,
        InvoiceController invoiceController,
        PaymentController paymentController,
        BurgerController burgerController,
        PrototypeController prototypeController,
        SingletonController singletonController,
        FlyweightController flyweightController,
        DecoratorController decoratorController,
        BridgeController bridgeController,
        ProxyController proxyController,
        MainMenuView view)
    {
        _adapterController = adapterController;
        _compositeController = compositeController;
        _facadeController = facadeController;
        _invoiceController = invoiceController;
        _paymentController = paymentController;
        _burgerController = burgerController;
        _prototypeController = prototypeController;
        _singletonController = singletonController;
        _flyweightController = flyweightController;
        _decoratorController = decoratorController;
        _bridgeController = bridgeController;
        _proxyController = proxyController;
        _view = view;
    }

    public void Run()
    {
        var running = true;
        while (running)
        {
            _view.ShowHeader();
            _view.ShowMainMenu();
            var choice = _view.ReadChoice();

            switch (choice)
            {
                case "1":
                    _adapterController.RunPaymentGatewayDemo();
                    break;
                case "2":
                    _compositeController.RunPaymentBatchDemo();
                    break;
                case "3":
                    _facadeController.RunCheckoutDemo();
                    break;
                case "4":
                    _invoiceController.CreateInvoice();
                    break;
                case "5":
                    _paymentController.CreatePayment();
                    break;
                case "6":
                    _paymentController.ProcessPayment();
                    break;
                case "7":
                    _invoiceController.ListInvoices();
                    break;
                case "8":
                    _burgerController.CreateBurger();
                    break;
                case "9":
                    _prototypeController.RunLaptopPrototypeDemo();
                    break;
                case "10":
                    _singletonController.RunSingletonDemo();
                    break;
                case "11":
                    _flyweightController.RunPaymentFlyweightDemo();
                    break;
                case "12":
                    _decoratorController.RunNotificationDecoratorDemo();
                    break;
                case "13":
                    _bridgeController.RunBridgeDemo();
                    break;
                case "14":
                    _proxyController.RunProxyDemo();
                    break;
                case "15":
                    running = false;
                    break;
                default:
                    _view.ShowMessage("Unknown option.");
                    break;
            }
        }
    }
}
