using TMPPP.Views;

namespace TMPPP.Controllers;

public class AppController
{
    private readonly InvoiceController _invoiceController;
    private readonly PaymentController _paymentController;
    private readonly BurgerController _burgerController;
    private readonly PrototypeController _prototypeController;
    private readonly SingletonController _singletonController;
    private readonly MainMenuView _view;

    public AppController(
        InvoiceController invoiceController,
        PaymentController paymentController,
        BurgerController burgerController,
        PrototypeController prototypeController,
        SingletonController singletonController,
        MainMenuView view)
    {
        _invoiceController = invoiceController;
        _paymentController = paymentController;
        _burgerController = burgerController;
        _prototypeController = prototypeController;
        _singletonController = singletonController;
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
                    _invoiceController.CreateInvoice();
                    break;
                case "2":
                    _paymentController.CreatePayment();
                    break;
                case "3":
                    _paymentController.ProcessPayment();
                    break;
                case "4":
                    _invoiceController.ListInvoices();
                    break;
                case "5":
                    _burgerController.CreateBurger();
                    break;
                case "6":
                    _prototypeController.RunLaptopPrototypeDemo();
                    break;
                case "7":
                    _singletonController.RunSingletonDemo();
                    break;
                case "8":
                    running = false;
                    break;
                default:
                    _view.ShowMessage("Unknown option.");
                    break;
            }
        }
    }
}
