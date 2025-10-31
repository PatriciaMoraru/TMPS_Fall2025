using Bakery.Services;
using System;

namespace Bakery.Domain;
public abstract class Bread
{ 
    public abstract string Type { get; }
    public abstract decimal Price { get; }
    protected abstract MachineService Service { get; }

    public void Bake()
    {
        BakeryLogger.Instance.Log($"Starting to bake {Type} bread...");

        Console.WriteLine($"Preparing {Type} bread...");
        Service.OpenDoor();
        Service.CookBread();
        Service.CloseDoor();
        Console.WriteLine($"{Type} bread baked at {Price:C}.");

        BakeryLogger.Instance.Log($"{Type} bread finished.\n");
    }
}
