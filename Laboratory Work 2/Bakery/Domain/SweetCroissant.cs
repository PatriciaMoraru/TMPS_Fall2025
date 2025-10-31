using Bakery.Models;
using Bakery.Services;

namespace Bakery.Domain;

public class SweetCroissant : Croissant
{
    public SweetCroissant(MachineService service, decimal price, CroissantCustomization customization)
    {
        Service = service;
        Price = price;
        Customization = customization;
    }

    public override CroissantType Type => CroissantType.Sweet;
    public override decimal Price { get; }

    public CroissantSugarAmount Sugar
    {
        get => Customization.Sugar;
        set => Customization.Sugar = value;
    }

    protected override MachineService Service { get; }
    protected sealed override CroissantCustomization Customization { get; set; }

    protected override void CustomOperation()
    {
        Console.WriteLine($"Adding {Sugar} grams of sugar...");
    }
}