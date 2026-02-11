using TMPPP.Domain.Interfaces;
using TMPPP.Domain.Processors;
using TMPPP.Domain.Services;
using TMPPP.Infrastructure.InMemory;
using TMPPP.Infrastructure.Notifications;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<InMemoryPaymentRepository>();
builder.Services.AddSingleton<InMemoryInvoiceRepository>();
builder.Services.AddSingleton<IPaymentRepository>(sp => sp.GetRequiredService<InMemoryPaymentRepository>());
builder.Services.AddSingleton<IInvoiceRepository>(sp => sp.GetRequiredService<InMemoryInvoiceRepository>());
builder.Services.AddSingleton<INotificationService, ConsoleNotificationService>();
builder.Services.AddSingleton<IPaymentProcessor, PaymentProcessor>();
builder.Services.AddSingleton<IPaymentService, PaymentService>();

var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run("http://localhost:5055");
