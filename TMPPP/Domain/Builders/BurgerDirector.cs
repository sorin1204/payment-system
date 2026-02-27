namespace TMPPP.Domain.Builders;

public class BurgerDirector
{
    private readonly IBurgerBuilder _builder;

    public BurgerDirector(IBurgerBuilder builder)
    {
        _builder = builder;
    }

    public Burger BuildClassicCombo()
    {
        return _builder
            .Reset()
            .WithBun("Classic")
            .WithPatty("Beef")
            .AddCheese()
            .AddTopping("Lettuce")
            .AddTopping("Tomato")
            .AddSauce("Ketchup")
            .AddFries()
            .AddDrink()
            .Build();
    }

    public Burger BuildVeggie()
    {
        return _builder
            .Reset()
            .WithBun("Whole grain")
            .WithPatty("Veggie")
            .AddTopping("Lettuce")
            .AddTopping("Onion")
            .AddTopping("Pickles")
            .AddSauce("Vegan mayo")
            .Build();
    }
}
