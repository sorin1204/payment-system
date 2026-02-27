using TMPPP.Domain.Builders;
using TMPPP.Views;

namespace TMPPP.Controllers;

public class BurgerController
{
    private readonly MainMenuView _view;
    private readonly BurgerDirector _director;
    private readonly IBurgerBuilder _builder;

    public BurgerController(MainMenuView view)
    {
        _view = view;
        _builder = new BurgerBuilder();
        _director = new BurgerDirector(_builder);
    }

    public void CreateBurger()
    {
        _view.ShowMessage("Builder demo:");
        _view.ShowMessage("1) Preset: Classic combo (Director)");
        _view.ShowMessage("2) Preset: Veggie (Director)");
        _view.ShowMessage("3) Custom builder (step by step)");

        var option = _view.Prompt("Choose builder mode: ");
        Burger burger = option switch
        {
            "1" => _director.BuildClassicCombo(),
            "2" => _director.BuildVeggie(),
            _ => BuildCustomBurger()
        };

        _view.ShowBurger(burger);
    }

    private Burger BuildCustomBurger()
    {
        var bun = _view.Prompt("Bun type (Classic/Brioche/Whole grain): ", "Classic");
        var patty = _view.Prompt("Patty type (Beef/Chicken/Veggie): ", "Beef");
        var cheeseAnswer = _view.Prompt("Add cheese? (y/n): ", "n");
        var toppings = _view.Prompt("Toppings (comma separated): ");
        var sauces = _view.Prompt("Sauces (comma separated): ");
        var friesAnswer = _view.Prompt("Add fries? (y/n): ", "n");
        var drinkAnswer = _view.Prompt("Add drink? (y/n): ", "n");

        var fluentBuilder = _builder
            .Reset()
            .WithBun(bun)
            .WithPatty(patty);

        if (IsYes(cheeseAnswer))
        {
            fluentBuilder.AddCheese();
        }

        foreach (var topping in SplitCsv(toppings))
        {
            fluentBuilder.AddTopping(topping);
        }

        foreach (var sauce in SplitCsv(sauces))
        {
            fluentBuilder.AddSauce(sauce);
        }

        if (IsYes(friesAnswer))
        {
            fluentBuilder.AddFries();
        }

        if (IsYes(drinkAnswer))
        {
            fluentBuilder.AddDrink();
        }

        return fluentBuilder.Build();
    }

    private static bool IsYes(string value)
    {
        return value.Trim().ToLowerInvariant() is "y" or "yes" or "da";
    }

    private static IEnumerable<string> SplitCsv(string csv)
    {
        return csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
}
