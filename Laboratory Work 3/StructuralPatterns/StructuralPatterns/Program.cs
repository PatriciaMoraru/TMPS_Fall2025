using StructuralPatterns.Facade;
using StructuralPatterns.Utilities;

namespace StructuralPatterns;

class Program
{
    static void Main(string[] args)
    {
        // Build demo catalog (Composite + Decorator)
        var catalogRoot = GiftShopBootstrapper.CreateDemoCatalog();

        // Facade for interacting with the system
        var facade = new GiftShopFacade(catalogRoot);

        RunMenu(facade);
    }

    private static void RunMenu(GiftShopFacade facade)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("==================================");
            Console.WriteLine("         GIFT SHOP MENU");
            Console.WriteLine("==================================");
            Console.WriteLine("1. Show full catalog");
            Console.WriteLine("2. Show price of a gift box");
            Console.WriteLine("3. Show detailed contents of a gift box");
            Console.WriteLine("4. Exit");
            Console.WriteLine("==================================");
            Console.Write("Choose an option (1-4): ");

            var input = Console.ReadLine();
            Console.WriteLine();

            switch (input)
            {
                case "1":
                    Console.WriteLine(">>> Catalog");
                    Console.WriteLine();
                    facade.ShowCatalog();
                    Pause();
                    break;

                case "2":
                    Console.Write("Enter the exact name of the gift box (e.g. 'Birthday Box', 'Christmas Box'): ");
                    {
                        var boxName = Console.ReadLine() ?? string.Empty;
                        Console.WriteLine();
                        facade.GetBoxPrice(boxName);
                        Pause();
                    }
                    break;

                case "3":
                    Console.Write("Enter the exact name of the gift box to see detailed contents: ");
                    {
                        var boxName = Console.ReadLine() ?? string.Empty;
                        Console.WriteLine();
                        facade.ShowBoxDetails(boxName);
                        Pause();
                    }
                    break;

                case "4":
                    Console.WriteLine("Goodbye! XO");
                    return;

                default:
                    Console.WriteLine("Invalid option. Please choose 1, 2, 3, or 4.");
                    Pause();
                    break;
            }
        }
    }

    private static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}
