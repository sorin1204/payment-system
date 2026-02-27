namespace TMPPP.Domain.Builders;

public class BurgerBuilder : IBurgerBuilder
{
    private Burger _burger = new();

    public IBurgerBuilder Reset()
    {
        _burger = new Burger();
        return this;
    }

    public IBurgerBuilder WithBun(string bunType)
    {
        if (!string.IsNullOrWhiteSpace(bunType))
        {
            _burger.BunType = bunType.Trim();
        }

        return this;
    }

    public IBurgerBuilder WithPatty(string pattyType)
    {
        if (!string.IsNullOrWhiteSpace(pattyType))
        {
            _burger.PattyType = pattyType.Trim();
        }

        return this;
    }

    public IBurgerBuilder AddCheese()
    {
        _burger.HasCheese = true;
        return this;
    }

    public IBurgerBuilder AddTopping(string topping)
    {
        if (!string.IsNullOrWhiteSpace(topping))
        {
            _burger.Toppings.Add(topping.Trim());
        }

        return this;
    }

    public IBurgerBuilder AddSauce(string sauce)
    {
        if (!string.IsNullOrWhiteSpace(sauce))
        {
            _burger.Sauces.Add(sauce.Trim());
        }

        return this;
    }

    public IBurgerBuilder AddFries()
    {
        _burger.HasFries = true;
        return this;
    }

    public IBurgerBuilder AddDrink()
    {
        _burger.HasDrink = true;
        return this;
    }

    public Burger Build()
    {
        var result = _burger;
        _burger = new Burger();
        return result;
    }
}
