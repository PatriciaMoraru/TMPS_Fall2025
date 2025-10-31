using System;

namespace Bakery.Domain;

public class CustomCake
{
    public string BaseLayer { get; set; } = "Vanilla Sponge";
    public string Cream { get; set; } = "Whipped Cream";
    public string Decoration { get; set; } = "Strawberries";
    public decimal Price { get; set; } = 10m;

    public void Display()
    {
        Console.WriteLine($"Custom Cake:");
        Console.WriteLine($" - Base: {BaseLayer}");
        Console.WriteLine($" - Cream: {Cream}");
        Console.WriteLine($" - Decoration: {Decoration}");
        Console.WriteLine($" - Total Price: {Price} lei\n");
    }
}