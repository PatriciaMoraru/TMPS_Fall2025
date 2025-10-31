using Bakery.Services;
using System;

namespace Bakery.Domain;

public abstract class Cake
{
    public abstract string Type { get; }
    public abstract decimal Price { get; }
    protected abstract MachineService Service { get; }

    public void Bake()
    {
        BakeryLogger.Instance.Log($"Starting to decorate {Type} cake...");

        Console.WriteLine($"Decorating {Type} cake...");
        Service.OpenDoor();
        Service.CookCake();
        Service.CloseDoor();
        Console.WriteLine($"{Type} cake ready for sale at {Price:C}.");

        BakeryLogger.Instance.Log($"{Type} cake finished.\n");
    }
}