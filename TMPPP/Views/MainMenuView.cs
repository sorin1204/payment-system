using TMPPP.Domain.Entities;
using TMPPP.Domain.Builders;

namespace TMPPP.Views;

public class MainMenuView
{
    public void ShowHeader()
    {
        Console.WriteLine("=== Payment Management (SOLID) ===");
    }

    public void ShowMainMenu()
    {
        Console.WriteLine("1) Create invoice");
        Console.WriteLine("2) Create payment");
        Console.WriteLine("3) Process payment");
        Console.WriteLine("4) List invoices");
        Console.WriteLine("5) Builder demo (custom burger)");
        Console.WriteLine("6) Exit");
    }

    public string? ReadChoice()
    {
        Console.Write("Choose: ");
        return Console.ReadLine();
    }

    public string Prompt(string label, string? fallback = null)
    {
        Console.Write(label);
        var input = Console.ReadLine();
        return string.IsNullOrWhiteSpace(input) ? (fallback ?? string.Empty) : input;
    }

    public void ShowMessage(string message)
    {
        Console.WriteLine(message);
    }

    public void ShowInvoiceCreated(Invoice invoice)
    {
        Console.WriteLine($"Invoice created: {invoice.Id}");
        Console.WriteLine($"Customer: {invoice.CustomerId}");
        Console.WriteLine($"Total: {invoice.Total.Amount} {invoice.Total.Currency}");
        Console.WriteLine($"Due: {invoice.DueDate:yyyy-MM-dd}");
    }

    public void ShowPaymentCreated(Guid paymentId)
    {
        Console.WriteLine($"Payment created: {paymentId}");
    }

    public void ShowPaymentResult(bool success, string message)
    {
        Console.WriteLine(success ? "Payment OK." : "Payment failed.");
        Console.WriteLine(message);
    }

    public void ShowInvoices(IReadOnlyCollection<Invoice> invoices)
    {
        if (invoices.Count == 0)
        {
            Console.WriteLine("No invoices yet.");
            return;
        }

        foreach (var invoice in invoices)
        {
            Console.WriteLine(
                $"{invoice.Id} | {invoice.Total.Amount} {invoice.Total.Currency} | Due: {invoice.DueDate:yyyy-MM-dd}");
        }
    }

    public void ShowBurger(Burger burger)
    {
        Console.WriteLine("Burger built successfully:");
        Console.WriteLine($"Bun: {burger.BunType}");
        Console.WriteLine($"Patty: {burger.PattyType}");
        Console.WriteLine($"Cheese: {(burger.HasCheese ? "Yes" : "No")}");
        Console.WriteLine(
            $"Toppings: {(burger.Toppings.Count == 0 ? "None" : string.Join(", ", burger.Toppings))}");
        Console.WriteLine(
            $"Sauces: {(burger.Sauces.Count == 0 ? "None" : string.Join(", ", burger.Sauces))}");
        Console.WriteLine($"Fries: {(burger.HasFries ? "Yes" : "No")}");
        Console.WriteLine($"Drink: {(burger.HasDrink ? "Yes" : "No")}");
        Console.WriteLine($"Estimated price: {burger.CalculatePrice():0.00} RON");
    }
}
