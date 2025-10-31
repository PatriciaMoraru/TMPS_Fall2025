using Bakery.Factory;
using Bakery.Domain;
using Bakery.Services;
using Bakery.Models;
using System;

namespace Bakery.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var service = new MachineService();

            Console.WriteLine("Welcome to the Bakery!");
            Console.WriteLine("Choose a demonstration:");
            Console.WriteLine("1. Abstract Factory (Sweet/Chocolate Family)");
            Console.WriteLine("2. Builder (Custom Cake)");
            Console.WriteLine("3. Prototype (Recipe Cloning)");
            Console.WriteLine("4. Object Pool (Machine Reuse)");
            Console.WriteLine("5. Factory Method (Pastry)");
            Console.WriteLine("6. Show All");
            Console.Write("Enter your choice (1–6): ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    RunAbstractFactory(service);
                    break;
                case "2":
                    RunBuilder();
                    break;
                case "3":
                    RunPrototype();
                    break;
                case "4":
                    RunObjectPool();
                    break;
                case "5":
                    RunFactoryMethod();
                    break;
                case "6":
                    RunAbstractFactory(service);
                    RunBuilder();
                    RunPrototype();
                    RunObjectPool();
                    RunFactoryMethod();
                    break;
                default:
                    Console.WriteLine("Invalid option. Goodbye!");
                    break;
            }

            Console.WriteLine("\nProgram finished. Thank you for visiting the bakery!");
        }

        // ======== ABSTRACT FACTORY ========
        static void RunAbstractFactory(MachineService service)
        {
            Console.WriteLine("\n--- ABSTRACT FACTORY PATTERN ---");
            Console.WriteLine("Choose a bakery family:");
            Console.WriteLine("1. Sweet");
            Console.WriteLine("2. Chocolate");
            Console.Write("Enter choice (1 or 2): ");
            string familyChoice = Console.ReadLine() ?? "1";

            IBakeryFactory factory = familyChoice switch
            {
                "1" => new SweetBakeryFactory(),
                "2" => new ChocolateBakeryFactory(),
                _ => new SweetBakeryFactory()
            };

            Console.WriteLine("\nWhat would you like to bake?");
            Console.WriteLine("1. Croissant");
            Console.WriteLine("2. Bread");
            Console.WriteLine("3. Cake");
            Console.WriteLine("4. Everything");
            Console.Write("Enter choice: ");
            string productChoice = Console.ReadLine() ?? "4";

            var customization = new CroissantCustomization();

            switch (productChoice)
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

            Console.WriteLine("Abstract Factory demonstration complete.\n");
        }

        // ======== BUILDER ========
        static void RunBuilder()
        {
            Console.WriteLine("\n--- BUILDER PATTERN ---");
            var builder = new CustomCakeBuilder();
            var director = new CakeDirector();

            Console.WriteLine("Choose type of custom cake:");
            Console.WriteLine("1. Chocolate Cake");
            Console.WriteLine("2. Birthday Cake");
            Console.Write("Enter choice: ");
            var cakeChoice = Console.ReadLine();

            CustomCake cake = cakeChoice switch
            {
                "1" => director.BuildChocolateCake(builder),
                "2" => director.BuildBirthdayCake(builder),
                _ => builder.Build()
            };

            cake.Display();
            Console.WriteLine("Builder demonstration complete.\n");
        }

        // ======== PROTOTYPE ========
        static void RunPrototype()
        {
            Console.WriteLine("\n--- PROTOTYPE PATTERN ---");
            var baseRecipe = new Recipe("Chocolate Croissant", "Flour, Butter, Chocolate");
            var clonedRecipe = (Recipe)baseRecipe.Clone();
            clonedRecipe.Name = "Almond Croissant";
            clonedRecipe.Ingredients += ", Almonds";

            baseRecipe.Display();
            clonedRecipe.Display();

            Console.WriteLine("Prototype demonstration complete.\n");
        }

        // ======== OBJECT POOL ========
        static void RunObjectPool()
        {
            Console.WriteLine("\n--- OBJECT POOL PATTERN ---");
            var pool = new MachineServicePool(2);

            var m1 = pool.Acquire();
            var m2 = pool.Acquire();
            pool.Release(m1);

            Console.WriteLine("Object Pool demonstration complete.\n");
        }

        // ======== FACTORY METHOD ========
        static void RunFactoryMethod()
        {
            Console.WriteLine("\n--- FACTORY METHOD PATTERN ---");
            PastryFactory croissantFactory = new CroissantFactoryMethod();
            PastryFactory muffinFactory = new MuffinFactoryMethod();

            croissantFactory.BakePastry();
            muffinFactory.BakePastry();

            Console.WriteLine("Factory Method demonstration complete.\n");
        }
    }
}
