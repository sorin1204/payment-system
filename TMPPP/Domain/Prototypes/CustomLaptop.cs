namespace TMPPP.Domain.Prototypes;

public sealed class CustomLaptop : IPrototype<CustomLaptop>
{
    public CustomLaptop(string model, LaptopConfiguration configuration, List<string> accessories)
    {
        Model = model;
        Configuration = configuration;
        Accessories = accessories;
    }

    public string Model { get; set; }
    public LaptopConfiguration Configuration { get; set; }
    public List<string> Accessories { get; set; }

    public CustomLaptop CloneShallow()
    {
        return (CustomLaptop)MemberwiseClone();
    }

    public CustomLaptop CloneDeep()
    {
        return new CustomLaptop(
            Model,
            Configuration.Copy(),
            new List<string>(Accessories));
    }
}
