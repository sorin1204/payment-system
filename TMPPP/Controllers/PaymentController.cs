using TMPPP.Domain.Factories;
using TMPPP.Domain.Interfaces;
using TMPPP.Domain.Services;
using TMPPP.Domain.ValueObjects;
using TMPPP.Views;

namespace TMPPP.Controllers;

public class PaymentController
{
    private readonly IPaymentService _paymentService;
    private readonly MainMenuView _view;

    public PaymentController(IPaymentService paymentService, MainMenuView view)
    {
        _paymentService = paymentService;
        _view = view;
    }

    public void CreatePayment()
    {
        var invoiceInput = _view.Prompt("Invoice id: ");
        if (!Guid.TryParse(invoiceInput, out var invoiceId))
        {
            _view.ShowMessage("Invalid invoice id.");
            return;
        }

        var amountInput = _view.Prompt("Amount: ", "0");
        if (!decimal.TryParse(amountInput, out var amount))
        {
            _view.ShowMessage("Invalid amount.");
            return;
        }

        var currency = _view.Prompt("Currency (e.g., RON): ", "RON");
        var payment = _paymentService.CreatePayment(invoiceId, new Money(amount, currency));
        _view.ShowPaymentCreated(payment.Id);
    }

    public void ProcessPayment()
    {
        var paymentInput = _view.Prompt("Payment id: ");
        if (!Guid.TryParse(paymentInput, out var paymentId))
        {
            _view.ShowMessage("Invalid payment id.");
            return;
        }

        _view.ShowMessage("Method: 1) Card  2) Bank transfer  3) Cash");
        var methodChoice = _view.Prompt("Choose: ");
        var creator = ResolveCreator(methodChoice);
        var method = creator.CreatePaymentMethod();

        var result = _paymentService.ProcessPayment(paymentId, method);
        _view.ShowPaymentResult(result.Success, result.Message);
    }

    private static PaymentMethodCreator ResolveCreator(string methodChoice)
    {
        return methodChoice switch
        {
            "1" => new CardPaymentMethodCreator(),
            "2" => new BankTransferPaymentMethodCreator(),
            _ => new CashPaymentMethodCreator()
        };
    }
}
