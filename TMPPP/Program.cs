using TMPPP.Controllers;
using TMPPP.Domain.Factories.AbstractFactory;
using TMPPP.Views;

using var factory = CreateFactory();

var paymentRepository = factory.CreatePaymentRepository();
var invoiceRepository = factory.CreateInvoiceRepository();
var notificationService = factory.CreateNotificationService();
var paymentProcessor = factory.CreatePaymentProcessor(paymentRepository, notificationService);
var paymentService = factory.CreatePaymentService(paymentRepository, invoiceRepository, paymentProcessor);

var view = new MainMenuView();
var invoiceController = new InvoiceController(invoiceRepository, view);
var paymentController = new PaymentController(paymentService, view);
var appController = new AppController(invoiceController, paymentController, view);

appController.Run();

static PaymentDomainFactory CreateFactory()
{
    var mode = Environment.GetEnvironmentVariable("PAYMENT_STORAGE")?.Trim().ToLowerInvariant();
    return mode == "memory"
        ? new InMemoryPaymentDomainFactory()
        : new SqlitePaymentDomainFactory("Data Source=payments.db");
}
