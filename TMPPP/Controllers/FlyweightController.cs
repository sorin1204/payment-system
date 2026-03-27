using TMPPP.Domain.Structural.Flyweight;
using TMPPP.Views;

namespace TMPPP.Controllers;

public class FlyweightController
{
    private readonly MainMenuView _view;

    public FlyweightController(MainMenuView view)
    {
        _view = view;
    }

    public void RunPaymentFlyweightDemo()
    {
        var result = BuildPaymentFlyweightDemo();
        _view.ShowFlyweightDemo(result);
    }

    public static FlyweightDemoResult BuildPaymentFlyweightDemo()
    {
        var factory = new PaymentDisplayProfileFactory();
        var entries = new List<PaymentDocumentEntry>();
        var definitions = new[]
        {
            new { Method = "card", Currency = "RON", Status = "processed", Channel = "POS Gateway", Footer = "Card settlement in 24h" },
            new { Method = "bank", Currency = "RON", Status = "pending", Channel = "IBAN Transfer", Footer = "Bank confirmation required" },
            new { Method = "cash", Currency = "RON", Status = "processed", Channel = "Cash Desk", Footer = "Fiscal receipt issued locally" },
            new { Method = "card", Currency = "EUR", Status = "processed", Channel = "POS Gateway", Footer = "Card settlement in 24h" },
            new { Method = "bank", Currency = "EUR", Status = "pending", Channel = "SEPA Transfer", Footer = "Bank confirmation required" },
            new { Method = "cash", Currency = "EUR", Status = "processed", Channel = "Cash Desk", Footer = "Fiscal receipt issued locally" }
        };

        const int repetitionsPerProfile = 400;
        var baseDate = new DateTime(2026, 3, 27, 8, 0, 0, DateTimeKind.Utc);

        for (var profileIndex = 0; profileIndex < definitions.Length; profileIndex++)
        {
            var definition = definitions[profileIndex];
            for (var i = 0; i < repetitionsPerProfile; i++)
            {
                var profile = factory.GetOrCreate(
                    definition.Method,
                    definition.Currency,
                    definition.Status,
                    definition.Channel,
                    definition.Footer);

                entries.Add(new PaymentDocumentEntry(
                    $"PAY-{profileIndex + 1:00}-{i + 1:0000}",
                    $"Customer {(i % 25) + 1}",
                    90 + profileIndex * 25 + (i % 7) * 5,
                    baseDate.AddMinutes(profileIndex * repetitionsPerProfile + i),
                    profile));
            }
        }

        var firstCardEntry = entries.First(x =>
            x.Profile.PaymentMethod == "card" &&
            x.Profile.Currency == "RON" &&
            x.Profile.Status == "processed");
        var anotherCardEntry = entries.Skip(10).First(x =>
            x.Profile.PaymentMethod == "card" &&
            x.Profile.Currency == "RON" &&
            x.Profile.Status == "processed");

        return new FlyweightDemoResult(
            entries,
            factory.SharedInstanceCount,
            entries.Count - factory.SharedInstanceCount,
            ReferenceEquals(firstCardEntry.Profile, anotherCardEntry.Profile),
            "Platile pastreaza separat datele variabile (referinta, client, suma), iar atributele repetate ale documentului sunt partajate prin factory.");
    }
}
