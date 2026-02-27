using TMPPP.Domain.Prototypes;
using TMPPP.Views;

namespace TMPPP.Controllers;

public class PrototypeController
{
    private readonly MainMenuView _view;

    public PrototypeController(MainMenuView view)
    {
        _view = view;
    }

    public void RunLaptopPrototypeDemo()
    {
        var prototype = new CustomLaptop(
            "DevBook Pro",
            new LaptopConfiguration("Intel i7", 16, 512),
            new List<string> { "Backpack", "Wireless Mouse" });

        var shallowCopy = prototype.CloneShallow();
        var deepCopy = prototype.CloneDeep();

        shallowCopy.Configuration.RamGb = 32;
        shallowCopy.Accessories.Add("USB-C Dock");

        deepCopy.Configuration.StorageGb = 1024;
        deepCopy.Accessories.Add("External SSD");

        _view.ShowPrototypeComparison(prototype, shallowCopy, deepCopy);
    }
}
