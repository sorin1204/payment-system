using Microsoft.EntityFrameworkCore;
using TMPPP.Domain.Entities;
using TMPPP.Domain.Interfaces;
using TMPPP.Domain.PaymentMethods;
using TMPPP.Domain.Processors;
using TMPPP.Domain.Services;
using TMPPP.Domain.ValueObjects;
using TMPPP.Infrastructure.Data;
using TMPPP.Infrastructure.Notifications;

var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite("Data Source=payments.db")
    .Options;
using var dbContext = new AppDbContext(dbOptions);
dbContext.Database.EnsureCreated();

var paymentRepository = new EfPaymentRepository(dbContext);
var invoiceRepository = new EfInvoiceRepository(dbContext);
var notificationService = new ConsoleNotificationService();
var paymentProcessor = new PaymentProcessor(paymentRepository, notificationService);
var paymentService = new PaymentService(paymentRepository, invoiceRepository, paymentProcessor);

Console.WriteLine("=== Payment Management (SOLID) ===");
Console.WriteLine("1) Create invoice");
Console.WriteLine("2) Create payment");
Console.WriteLine("3) Process payment");
Console.WriteLine("4) Exit");

var running = true;
while (running)
{
    Console.Write("Choose: ");
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            CreateInvoice(invoiceRepository);
            break;
        case "2":
            CreatePayment(paymentService);
            break;
        case "3":
            ProcessPayment(paymentService);
            break;
        case "4":
            running = false;
            break;
        default:
            Console.WriteLine("Unknown option.");
            break;
    }
}

static void CreateInvoice(IInvoiceRepository invoiceRepository)
{
    Console.Write("Customer id (blank for random): ");
    var customerInput = Console.ReadLine();
    var customerId = string.IsNullOrWhiteSpace(customerInput)
        ? Guid.NewGuid()
        : Guid.Parse(customerInput);

    Console.Write("Total amount: ");
    var amount = decimal.Parse(Console.ReadLine() ?? "0");
    Console.Write("Currency (e.g., RON): ");
    var currency = Console.ReadLine() ?? "RON";

    var invoice = new Invoice(Guid.NewGuid(), customerId, new Money(amount, currency), DateTime.UtcNow.AddDays(14));
    invoiceRepository.Add(invoice);
    Console.WriteLine($"Invoice created: {invoice.Id}");
}

static void CreatePayment(IPaymentService paymentService)
{
    Console.Write("Invoice id: ");
    var invoiceId = Guid.Parse(Console.ReadLine() ?? string.Empty);
    Console.Write("Amount: ");
    var amount = decimal.Parse(Console.ReadLine() ?? "0");
    Console.Write("Currency (e.g., RON): ");
    var currency = Console.ReadLine() ?? "RON";

    var payment = paymentService.CreatePayment(invoiceId, new Money(amount, currency));
    Console.WriteLine($"Payment created: {payment.Id}");
}

static void ProcessPayment(IPaymentService paymentService)
{
    Console.Write("Payment id: ");
    var paymentId = Guid.Parse(Console.ReadLine() ?? string.Empty);
    Console.WriteLine("Method: 1) Card  2) Bank transfer  3) Cash");
    Console.Write("Choose: ");
    var methodChoice = Console.ReadLine();

    IPaymentMethod method = methodChoice switch
    {
        "1" => new CardPaymentMethod("Demo User", "4242"),
        "2" => new BankTransferPaymentMethod("RO00BANK0000000000", "DemoBank"),
        _ => new CashPaymentMethod()
    };

    var result = paymentService.ProcessPayment(paymentId, method);
    Console.WriteLine(result.Success ? "Payment OK." : "Payment failed.");
    Console.WriteLine(result.Message);
}
