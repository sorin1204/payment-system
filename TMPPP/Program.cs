using Microsoft.AspNetCore.Mvc;
using TMPPP.Controllers;
using TMPPP.Domain.Entities;
using TMPPP.Domain.Factories;
using TMPPP.Domain.Factories.AbstractFactory;
using TMPPP.Domain.Services;
using TMPPP.Domain.ValueObjects;
using TMPPP.Views;

if (args.Any(x => string.Equals(x, "--api", StringComparison.OrdinalIgnoreCase)))
{
    RunApi(args);
    return;
}

RunConsole();

void RunConsole()
{
    using var factory = CreateFactory();

    var paymentRepository = factory.CreatePaymentRepository();
    var invoiceRepository = factory.CreateInvoiceRepository();
    var notificationService = factory.CreateNotificationService();
    var paymentProcessor = factory.CreatePaymentProcessor(paymentRepository, notificationService);
    var paymentService = factory.CreatePaymentService(paymentRepository, invoiceRepository, paymentProcessor);

    var view = new MainMenuView();
    var invoiceController = new InvoiceController(invoiceRepository, view);
    var paymentController = new PaymentController(paymentService, view);
    var burgerController = new BurgerController(view);
    var prototypeController = new PrototypeController(view);
    var appController = new AppController(
        invoiceController,
        paymentController,
        burgerController,
        prototypeController,
        view);

    appController.Run();
}

void RunApi(string[] runArgs)
{
    var builder = WebApplication.CreateBuilder(runArgs);
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    var factory = CreateFactory();
    app.Lifetime.ApplicationStopping.Register(factory.Dispose);

    var paymentRepository = factory.CreatePaymentRepository();
    var invoiceRepository = factory.CreateInvoiceRepository();
    var notificationService = factory.CreateNotificationService();
    var paymentProcessor = factory.CreatePaymentProcessor(paymentRepository, notificationService);
    var paymentService = factory.CreatePaymentService(paymentRepository, invoiceRepository, paymentProcessor);

    app.UseDefaultFiles();
    app.UseStaticFiles();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapGet("/api/health", () => Results.Ok(new { status = "ok", utc = DateTime.UtcNow }));

    app.MapGet("/api/info", () => Results.Ok(new
    {
        app = "TMPPP API",
        docs = "/swagger",
        ui = "/",
        storage = Environment.GetEnvironmentVariable("PAYMENT_STORAGE")?.Trim().ToLowerInvariant() == "memory"
            ? "memory"
            : "sqlite"
    }));

    app.MapGet("/api/payment-methods", () => Results.Ok(new[] { "card", "bank", "cash" }));

    app.MapPost("/api/invoices", ([FromBody] CreateInvoiceRequest request) =>
    {
        if (request.Amount <= 0)
        {
            return Results.BadRequest(new { error = "Amount must be greater than 0." });
        }

        var customerId = request.CustomerId ?? Guid.NewGuid();
        var currency = string.IsNullOrWhiteSpace(request.Currency) ? "RON" : request.Currency.Trim().ToUpperInvariant();
        var dueDate = request.DueDateUtc ?? DateTime.UtcNow.AddDays(14);

        var invoice = new Invoice(Guid.NewGuid(), customerId, new Money(request.Amount, currency), dueDate);
        invoiceRepository.Add(invoice);

        return Results.Ok(ToInvoiceDto(invoice));
    });

    app.MapGet("/api/invoices", () =>
    {
        var data = invoiceRepository
            .GetAll()
            .OrderByDescending(x => x.DueDate)
            .Select(ToInvoiceDto)
            .ToList();

        return Results.Ok(data);
    });

    app.MapPost("/api/payments", ([FromBody] CreatePaymentRequest request) =>
    {
        if (request.Amount <= 0)
        {
            return Results.BadRequest(new { error = "Amount must be greater than 0." });
        }

        if (invoiceRepository.GetById(request.InvoiceId) is null)
        {
            return Results.NotFound(new { error = "Invoice not found." });
        }

        var currency = string.IsNullOrWhiteSpace(request.Currency) ? "RON" : request.Currency.Trim().ToUpperInvariant();
        var payment = paymentService.CreatePayment(request.InvoiceId, new Money(request.Amount, currency));

        return Results.Ok(ToPaymentDto(payment));
    });

    app.MapPost("/api/payments/{paymentId:guid}/process", (Guid paymentId, [FromBody] ProcessPaymentRequest request) =>
    {
        try
        {
            var creator = ResolveCreator(request.Method);
            var method = creator.CreatePaymentMethod();
            var result = paymentService.ProcessPayment(paymentId, method);
            var payment = paymentRepository.GetById(paymentId);

            return Results.Ok(new
            {
                result.Success,
                result.Message,
                payment = payment is null ? null : ToPaymentDto(payment)
            });
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Results.NotFound(new { error = ex.Message });
        }
    });

    app.MapPost("/api/demo/run", () =>
    {
        var invoice = new Invoice(Guid.NewGuid(), Guid.NewGuid(), new Money(150m, "RON"), DateTime.UtcNow.AddDays(14));
        invoiceRepository.Add(invoice);

        var payment = paymentService.CreatePayment(invoice.Id, new Money(150m, "RON"));
        var result = paymentService.ProcessPayment(payment.Id, new CashPaymentMethodCreator().CreatePaymentMethod());
        var processedPayment = paymentRepository.GetById(payment.Id);

        return Results.Ok(new
        {
            invoice = ToInvoiceDto(invoice),
            payment = processedPayment is null ? ToPaymentDto(payment) : ToPaymentDto(processedPayment),
            result.Success,
            result.Message
        });
    });

    app.Run();
}

static PaymentDomainFactory CreateFactory()
{
    var mode = Environment.GetEnvironmentVariable("PAYMENT_STORAGE")?.Trim().ToLowerInvariant();
    return mode == "memory"
        ? new InMemoryPaymentDomainFactory()
        : new SqlitePaymentDomainFactory("Data Source=payments.db");
}

static PaymentMethodCreator ResolveCreator(string? methodChoice)
{
    return methodChoice?.Trim().ToLowerInvariant() switch
    {
        "1" or "card" => new CardPaymentMethodCreator(),
        "2" or "bank" or "banktransfer" => new BankTransferPaymentMethodCreator(),
        "3" or "cash" => new CashPaymentMethodCreator(),
        _ => throw new ArgumentException("Method must be one of: card, bank, cash.")
    };
}

static InvoiceDto ToInvoiceDto(Invoice invoice)
{
    return new InvoiceDto(invoice.Id, invoice.CustomerId, invoice.Total.Amount, invoice.Total.Currency, invoice.DueDate);
}

static PaymentDto ToPaymentDto(Payment payment)
{
    return new PaymentDto(
        payment.Id,
        payment.InvoiceId,
        payment.Amount.Amount,
        payment.Amount.Currency,
        payment.CreatedAt,
        payment.Status.ToString());
}

internal sealed record CreateInvoiceRequest(decimal Amount, string? Currency, Guid? CustomerId, DateTime? DueDateUtc);

internal sealed record CreatePaymentRequest(Guid InvoiceId, decimal Amount, string? Currency);

internal sealed record ProcessPaymentRequest(string Method);

internal sealed record InvoiceDto(Guid Id, Guid CustomerId, decimal TotalAmount, string Currency, DateTime DueDateUtc);

internal sealed record PaymentDto(Guid Id, Guid InvoiceId, decimal Amount, string Currency, DateTime CreatedAtUtc, string Status);
