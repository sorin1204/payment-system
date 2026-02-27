namespace TMPPP.Domain.Builders;

public interface IBurgerBuilder
{
    IBurgerBuilder Reset();
    IBurgerBuilder WithBun(string bunType);
    IBurgerBuilder WithPatty(string pattyType);
    IBurgerBuilder AddCheese();
    IBurgerBuilder AddTopping(string topping);
    IBurgerBuilder AddSauce(string sauce);
    IBurgerBuilder AddFries();
    IBurgerBuilder AddDrink();
    Burger Build();
}
