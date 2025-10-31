using Bakery.Factory;
using Bakery.Models;
using Bakery.Services;
using System;

namespace Bakery.Client;

internal class Program
{
    static void Main(string[] args)
    {
        var service = new MachineService();

        Console.WriteLine("Welcome to the Bakery!");
        Console.WriteLine("Choose your flavor family:");
        Console.WriteLine("1. Sweet");
        Console.WriteLine("2. Chocolate");
        Console.Write("Enter choice (1 or 2): ");
        string choice = Console.ReadLine() ?? "1";

        IBakeryFactory factory = choice switch
        {
            "1" => new SweetBakeryFactory(),
            "2" => new ChocolateBakeryFactory(),
            _ => new SweetBakeryFactory()
        };

        Console.WriteLine("\nPreparing your order...");

        Console.WriteLine("What would you like to bake?");
        Console.WriteLine("1. Croissant");
        Console.WriteLine("2. Bread");
        Console.WriteLine("3. Cake");
        Console.WriteLine("4. Everything!");
        Console.Write("Enter choice: ");
        string itemChoice = Console.ReadLine() ?? "4";

        var customization = new CroissantCustomization();

        switch (itemChoice)
        {
            case "1":
                factory.CreateCroissant(service, customization).Make();
                break;
            case "2":
                factory.CreateBread(service).Bake();
                break;
            case "3":
                factory.CreateCake(service).Bake();
                break;
            case "4":
                factory.CreateCroissant(service, customization).Make();
                factory.CreateBread(service).Bake();
                factory.CreateCake(service).Bake();
                break;
            default:
                Console.WriteLine("Invalid choice, nothing baked today!");
                break;
        }

        Console.WriteLine("\nOrder completed! Thank you for visiting the bakery!");
    }
}
