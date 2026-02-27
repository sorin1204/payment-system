namespace TMPPP.Domain.Builders;

public class Burger
{
    public string BunType { get; set; } = "Classic";
    public string PattyType { get; set; } = "Beef";
    public bool HasCheese { get; set; }
    public bool HasFries { get; set; }
    public bool HasDrink { get; set; }
    public List<string> Toppings { get; } = new();
    public List<string> Sauces { get; } = new();

    public decimal CalculatePrice()
    {
        var price = 15m;

        if (!string.Equals(BunType, "Classic", StringComparison.OrdinalIgnoreCase))
        {
            price += 1.5m;
        }

        if (!string.Equals(PattyType, "Beef", StringComparison.OrdinalIgnoreCase))
        {
            price += 2m;
        }

        if (HasCheese)
        {
            price += 2m;
        }

        price += Toppings.Count * 1m;
        price += Sauces.Count * 0.5m;

        if (HasFries)
        {
            price += 6m;
        }

        if (HasDrink)
        {
            price += 4m;
        }

        return price;
    }
}
