using Bakery.Models;
using Bakery.Services;

namespace Bakery.Domain;

public class ChocolateCroissant : Croissant
{
    public ChocolateCroissant(MachineService service, decimal price, CroissantCustomization customization)
    {
        Service = service;
        Price = price;
        Customization = customization;
    }

    public override CroissantType Type => CroissantType.Chocolate;
    public override decimal Price { get; }

    public CroissantChocolateAmount Chocolate
    {
        get => Customization.Chocolate;
        set => Customization.Chocolate = value;
    }

    protected override MachineService Service { get; }
    protected sealed override CroissantCustomization Customization { get; set; }

    protected override void CustomOperation()
    {
        Console.WriteLine($"Adding {Chocolate} grams of chocolate...");
    }
}