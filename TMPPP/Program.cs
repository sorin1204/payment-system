using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using TMPPP.Controllers;
using TMPPP.Domain.Entities;
using TMPPP.Domain.Factories;
using TMPPP.Domain.Factories.AbstractFactory;
using TMPPP.Domain.Services;
using TMPPP.Domain.Structural.Adapter;
using TMPPP.Domain.Structural.Bridge;
using TMPPP.Domain.Structural.Composite;
using TMPPP.Domain.Structural.Decorator;
using TMPPP.Domain.Structural.Facade;
using TMPPP.Domain.Structural.Flyweight;
using TMPPP.Domain.ValueObjects;
using TMPPP.Views;

var appRoot = ResolveAppRoot();
var sqliteConnectionString = $"Data Source={Path.Combine(appRoot, "payments.db")}";

if (args.Any(x => string.Equals(x, "--api", StringComparison.OrdinalIgnoreCase)))
{
    RunApi(args, appRoot, sqliteConnectionString);
    return;
}

RunConsole(appRoot, sqliteConnectionString);

void RunConsole(string rootPath, string defaultConnectionString)
{
    using var factory = CreateFactory(defaultConnectionString);

    var customerRepository = factory.CreateCustomerRepository();
    var paymentRepository = factory.CreatePaymentRepository();
    var invoiceRepository = factory.CreateInvoiceRepository();
    var notificationService = factory.CreateNotificationService();
    var paymentProcessor = factory.CreatePaymentProcessor(paymentRepository, notificationService);
    var paymentService = factory.CreatePaymentService(paymentRepository, invoiceRepository, paymentProcessor);
    _ = customerRepository;

    var view = new MainMenuView();
    var adapterController = new AdapterController(view);
    var compositeController = new CompositeController(view);
    var paymentCheckoutFacade = new PaymentCheckoutFacade(customerRepository, invoiceRepository, paymentService);
    var facadeController = new FacadeController(view, paymentCheckoutFacade);
    var invoiceController = new InvoiceController(invoiceRepository, view);
    var paymentController = new PaymentController(paymentService, view);
    var burgerController = new BurgerController(view);
    var prototypeController = new PrototypeController(view);
    var singletonController = new SingletonController(view, defaultConnectionString);
    var flyweightController = new FlyweightController(view);
    var decoratorController = new DecoratorController(view);
    var bridgeController = new BridgeController(view);
    var appController = new AppController(
        adapterController,
        compositeController,
        facadeController,
        invoiceController,
        paymentController,
        burgerController,
        prototypeController,
        singletonController,
        flyweightController,
        decoratorController,
        bridgeController,
        view);

    appController.Run();
}

void RunApi(string[] runArgs, string rootPath, string defaultConnectionString)
{
    var builder = WebApplication.CreateBuilder(new WebApplicationOptions
    {
        Args = runArgs,
        ContentRootPath = rootPath
    });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    var factory = CreateFactory(defaultConnectionString);
    app.Lifetime.ApplicationStopping.Register(factory.Dispose);

    var customerRepository = factory.CreateCustomerRepository();
    var paymentRepository = factory.CreatePaymentRepository();
    var invoiceRepository = factory.CreateInvoiceRepository();
    var notificationService = factory.CreateNotificationService();
    var paymentProcessor = factory.CreatePaymentProcessor(paymentRepository, notificationService);
    var paymentService = factory.CreatePaymentService(paymentRepository, invoiceRepository, paymentProcessor);
    var paymentCheckoutFacade = new PaymentCheckoutFacade(customerRepository, invoiceRepository, paymentService);
    var useMemoryStorage = Environment.GetEnvironmentVariable("PAYMENT_STORAGE")?.Trim().ToLowerInvariant() == "memory";

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

    app.MapGet("/api/patterns/adapter-demo", () =>
    {
        var request = new PaymentRequest(249.99m, "RON", "Laborator structural patterns");
        var gateways = new List<IOnlinePaymentGateway>
        {
            new PayPalAdapter(new PayPalGateway()),
            new StripeAdapter(new StripeGateway()),
            new GooglePayAdapter(new GooglePayGateway())
        };

        var responses = gateways
            .Select(gateway => gateway.Pay(request))
            .ToList();

        return Results.Ok(new
        {
            pattern = "Adapter",
            request,
            adapters = responses,
            explanation = "Aceeasi cerere este trimisa unitar catre gateway-uri cu API-uri diferite."
        });
    });

    app.MapGet("/api/patterns/composite-demo", () =>
    {
        var batch = CompositeController.BuildPaymentBatch();
        return Results.Ok(new
        {
            pattern = "Composite",
            componentType = "Payment batch hierarchy",
            totalAmount = batch.GetAmount(),
            structure = ToPaymentComponentDto(batch),
            rendered = batch.Render(),
            explanation = "Platile individuale si loturile de plati sunt tratate uniform prin aceeasi interfata."
        });
    });

    app.MapPost("/api/patterns/facade-checkout", ([FromBody] FacadeCheckoutApiRequest request) =>
    {
        try
        {
            var response = paymentCheckoutFacade.ExecuteCheckout(new CheckoutRequest(
                request.CustomerName,
                request.CustomerEmail,
                request.Amount,
                string.IsNullOrWhiteSpace(request.Currency) ? "RON" : request.Currency,
                request.PaymentMethod,
                request.DueDateUtc));

            return Results.Ok(new
            {
                pattern = "Facade",
                operation = "One-step checkout",
                response.CustomerId,
                response.InvoiceId,
                response.PaymentId,
                response.Success,
                response.Message,
                response.PaymentMethod,
                response.Amount,
                response.Currency,
                explanation = "Fațada ascunde pașii interni: client, factură, plată și procesare."
            });
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    });

    app.MapPost("/api/patterns/composite-build", ([FromBody] BuildCompositeBatchRequest request) =>
    {
        if (string.IsNullOrWhiteSpace(request.BatchName))
        {
            return Results.BadRequest(new { error = "BatchName is required." });
        }

        var currency = string.IsNullOrWhiteSpace(request.Currency) ? "RON" : request.Currency.Trim().ToUpperInvariant();
        var groupRequests = request.Groups?.ToList() ?? [];
        if (groupRequests.Count == 0)
        {
            return Results.BadRequest(new { error = "At least one payment group is required." });
        }

        try
        {
            var batch = BuildCompositeBatch(request.BatchName.Trim(), currency, groupRequests);
            return Results.Ok(new
            {
                pattern = "Composite",
                source = "custom-builder",
                totalAmount = batch.GetAmount(),
                structure = ToPaymentComponentDto(batch),
                rendered = batch.Render(),
                explanation = "Batch-ul principal si sub-batch-urile folosesc aceeasi interfata comuna."
            });
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    });

    app.MapGet("/api/patterns/flyweight-demo", () =>
    {
        var result = FlyweightController.BuildPaymentFlyweightDemo();
        return Results.Ok(new
        {
            pattern = "Flyweight",
            domain = "Payment document rendering",
            totalEntries = result.TotalEntries,
            uniqueSharedProfiles = result.UniqueFlyweights,
            avoidedDuplicateObjects = result.SavedObjects,
            sharedInstanceReused = result.CardProfileShared,
            sampleEntries = result.Entries.Take(5).Select(entry => new
            {
                entry.PaymentReference,
                entry.CustomerName,
                entry.Amount,
                entry.CreatedAtUtc,
                profile = new
                {
                    entry.Profile.PaymentMethod,
                    entry.Profile.Currency,
                    entry.Profile.Status,
                    entry.Profile.ProcessingChannel,
                    entry.Profile.ReceiptFooter,
                    profileHash = entry.Profile.GetHashCode()
                }
            }),
            explanation = result.Explanation
        });
    });

    app.MapGet("/api/patterns/decorator-demo", () =>
    {
        var result = DecoratorController.BuildNotificationDecoratorDemo();
        return Results.Ok(new
        {
            pattern = "Decorator",
            domain = "Notification delivery",
            result.Recipient,
            result.Subject,
            result.Message,
            channels = result.DeliveredChannels.Select(channel => new
            {
                channel.Name,
                channel.Details
            }),
            explanation = result.Explanation
        });
    });

    app.MapGet("/api/patterns/bridge-demo", () =>
    {
        var result = BridgeController.BuildBridgeDemo();
        return Results.Ok(new
        {
            pattern = "Bridge",
            domain = "Financial document rendering",
            combinations = result.Items.Select(item => new
            {
                item.DocumentType,
                item.Renderer,
                item.Output
            }),
            explanation = result.Explanation
        });
    });

    app.MapPost("/api/invoices", ([FromBody] CreateInvoiceRequest request) =>
    {
        if (request.Amount <= 0)
        {
            return Results.BadRequest(new { error = "Amount must be greater than 0." });
        }

        Guid customerId;
        if (string.IsNullOrWhiteSpace(request.CustomerId))
        {
            customerId = Guid.NewGuid();
        }
        else if (!Guid.TryParse(request.CustomerId, out customerId))
        {
            return Results.BadRequest(new { error = "CustomerId must be a valid GUID." });
        }

        var currency = string.IsNullOrWhiteSpace(request.Currency) ? "RON" : request.Currency.Trim().ToUpperInvariant();
        var dueDate = request.DueDateUtc ?? DateTime.UtcNow.AddDays(14);
        var autoName = string.IsNullOrWhiteSpace(request.CustomerName) ? $"Customer {customerId:N}" : request.CustomerName.Trim();
        var autoEmail = string.IsNullOrWhiteSpace(request.CustomerEmail) ? $"{customerId:N}@autogen.local" : request.CustomerEmail.Trim();
        if (useMemoryStorage)
        {
            customerRepository.EnsureExists(customerId, autoName, autoEmail);
        }
        else
        {
            EnsureCustomerExistsInSqlite(defaultConnectionString, customerId, autoName, autoEmail);
        }

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
        var customerId = Guid.NewGuid();
        if (useMemoryStorage)
        {
            customerRepository.EnsureExists(customerId, "Demo Customer", $"{customerId:N}@demo.local");
        }
        else
        {
            EnsureCustomerExistsInSqlite(
                defaultConnectionString,
                customerId,
                "Demo Customer",
                $"{customerId:N}@demo.local");
        }
        var invoice = new Invoice(Guid.NewGuid(), customerId, new Money(150m, "RON"), DateTime.UtcNow.AddDays(14));
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

static void EnsureCustomerExistsInSqlite(string connectionString, Guid customerId, string name, string email)
{
    using var connection = new SqliteConnection(connectionString);
    connection.Open();

    using var command = connection.CreateCommand();
    command.CommandText =
        "INSERT OR IGNORE INTO \"Customers\" (\"Id\", \"Name\", \"Email\") VALUES ($id, $name, $email);";
    command.Parameters.AddWithValue("$id", customerId.ToString().ToUpperInvariant());
    command.Parameters.AddWithValue("$name", name);
    command.Parameters.AddWithValue("$email", email);
    command.ExecuteNonQuery();
}

static PaymentDomainFactory CreateFactory(string sqliteConnectionString)
{
    var mode = Environment.GetEnvironmentVariable("PAYMENT_STORAGE")?.Trim().ToLowerInvariant();
    return mode == "memory"
        ? new InMemoryPaymentDomainFactory()
        : new SqlitePaymentDomainFactory(sqliteConnectionString);
}

static string ResolveAppRoot()
{
    var current = Directory.GetCurrentDirectory();
    if (Directory.Exists(Path.Combine(current, "wwwroot")))
    {
        return current;
    }

    var projectFolder = Path.Combine(current, "TMPPP");
    if (Directory.Exists(Path.Combine(projectFolder, "wwwroot")))
    {
        return projectFolder;
    }

    return current;
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

static object ToPaymentComponentDto(IPaymentComponent component)
{
    return component switch
    {
        PaymentLeaf item => new
        {
            type = "item",
            item.Name,
            amount = item.GetAmount(),
            item.Currency
        },
        PaymentBatch category => new
        {
            type = "batch",
            category.Name,
            amount = category.GetAmount(),
            category.Currency,
            children = category.Children.Select(ToPaymentComponentDto).ToList()
        },
        _ => new
        {
            type = "unknown",
            component.Name,
            amount = component.GetAmount()
        }
    };
}

static PaymentBatch BuildCompositeBatch(string batchName, string currency, IReadOnlyCollection<CompositeGroupRequest> groups)
{
    var root = new PaymentBatch(batchName, currency);

    foreach (var group in groups)
    {
        if (string.IsNullOrWhiteSpace(group.GroupName))
        {
            throw new ArgumentException("Each group must have a name.");
        }

        var payments = group.Payments?.ToList() ?? [];
        if (payments.Count == 0)
        {
            throw new ArgumentException($"Group '{group.GroupName}' must contain at least one payment.");
        }

        var groupBatch = new PaymentBatch(group.GroupName.Trim(), currency);
        foreach (var payment in payments)
        {
            if (string.IsNullOrWhiteSpace(payment.Name))
            {
                throw new ArgumentException($"A payment in group '{group.GroupName}' is missing a name.");
            }

            if (payment.Amount <= 0)
            {
                throw new ArgumentException($"Payment '{payment.Name}' must have an amount greater than 0.");
            }

            groupBatch.Add(new PaymentLeaf(payment.Name.Trim(), payment.Amount, currency));
        }

        root.Add(groupBatch);
    }

    return root;
}

internal sealed record CreateInvoiceRequest(
    decimal Amount,
    string? Currency,
    string? CustomerId,
    string? CustomerName,
    string? CustomerEmail,
    DateTime? DueDateUtc);

internal sealed record CreatePaymentRequest(Guid InvoiceId, decimal Amount, string? Currency);

internal sealed record ProcessPaymentRequest(string Method);

internal sealed record BuildCompositeBatchRequest(string BatchName, string? Currency, List<CompositeGroupRequest>? Groups);

internal sealed record CompositeGroupRequest(string GroupName, List<CompositePaymentRequest>? Payments);

internal sealed record CompositePaymentRequest(string Name, decimal Amount);

internal sealed record FacadeCheckoutApiRequest(
    string CustomerName,
    string CustomerEmail,
    decimal Amount,
    string? Currency,
    string PaymentMethod,
    DateTime? DueDateUtc);

internal sealed record InvoiceDto(Guid Id, Guid CustomerId, decimal TotalAmount, string Currency, DateTime DueDateUtc);

internal sealed record PaymentDto(Guid Id, Guid InvoiceId, decimal Amount, string Currency, DateTime CreatedAtUtc, string Status);
