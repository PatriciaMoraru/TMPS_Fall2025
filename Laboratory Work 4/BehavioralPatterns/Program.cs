using System;
using KitchenOrderApp.Domain;
using KitchenOrderApp.Patterns.Command;
using KitchenOrderApp.Patterns.Observer;
using KitchenOrderApp.Services;

namespace KitchenOrderApp;

class Program
{
    static void Main()
    {
        var repository = new OrderRepository();
        var notifier = new OrderNotifier();

        // Observers
        var waiter = new WaiterObserver();
        var kitchenDisplay = new KitchenDisplayObserver();

        notifier.StatusChanged += waiter.Update;
        notifier.StatusChanged += kitchenDisplay.Update;

        var service = new OrderService(repository, notifier);
        var invoker = new Invoker();

        Order? lastOrder = null;

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Restaurant Order Management (GoF style) ===");
            Console.WriteLine("1. Create new order");
            Console.WriteLine("2. Add demo items to last order");
            Console.WriteLine("3. Send last order to kitchen (CoR)");
            Console.WriteLine("4. Prepare last order (Command)");
            Console.WriteLine("5. Mark last order ready (Command)");
            Console.WriteLine("6. Serve last order (Command)");
            Console.WriteLine("7. Cancel last order (Command)");
            Console.WriteLine("8. List all orders");
            Console.WriteLine("9. Process order (Kitchen → Prepare → Ready)");
            Console.WriteLine("0. Exit");
            Console.Write("Choose option: ");

            var input = Console.ReadLine();
            Console.WriteLine();

            if (input == "0") break;

            switch (input)
            {
                case "1":
                    Console.Write("Table number: ");
                    if (int.TryParse(Console.ReadLine(), out var table))
                    {
                        lastOrder = service.CreateOrder(table);
                    }
                    break;

                case "2":
                    if (lastOrder == null)
                    {
                        Console.WriteLine("No last order.");
                        break;
                    }
                    lastOrder.Items.Add(new OrderItem("Cola", ItemCategory.Drink, 2));
                    lastOrder.Items.Add(new OrderItem("Lemonade", ItemCategory.Drink, 1));
                    lastOrder.Items.Add(new OrderItem("Steak", ItemCategory.Grill, 1));
                    lastOrder.Items.Add(new OrderItem("Burger", ItemCategory.Grill, 2));
                    lastOrder.Items.Add(new OrderItem("Cake", ItemCategory.Dessert, 1));
                    lastOrder.Items.Add(new OrderItem("Ice Cream", ItemCategory.Dessert, 2));
                    Console.WriteLine("Demo items added to last order.");
                    break;

                case "3":
                    if (lastOrder != null)
                        service.SendToKitchen(lastOrder);
                    else
                        Console.WriteLine("No last order.");
                    break;

                case "4":
                    if (lastOrder != null)
                    {
                        invoker.SetCommand(new PrepareOrderCommand(service, lastOrder));
                        invoker.Invoke();
                    }
                    else Console.WriteLine("No last order.");
                    break;

                case "5":
                    if (lastOrder != null)
                    {
                        invoker.SetCommand(new MarkReadyCommand(service, lastOrder));
                        invoker.Invoke();
                    }
                    else Console.WriteLine("No last order.");
                    break;

                case "6":
                    if (lastOrder != null)
                    {
                        invoker.SetCommand(new ServeOrderCommand(service, lastOrder));
                        invoker.Invoke();
                    }
                    else Console.WriteLine("No last order.");
                    break;

                case "7":
                    if (lastOrder != null)
                    {
                        invoker.SetCommand(new CancelOrderCommand(service, lastOrder));
                        invoker.Invoke();
                    }
                    else Console.WriteLine("No last order.");
                    break;

                case "8":
                    service.PrintOrders();
                    break;

                case "9":
                    if (lastOrder != null)
                    {
                        Console.WriteLine("[Workflow] Starting automated processing...");
                        service.SendToKitchen(lastOrder);
                        
                        invoker.SetCommand(new PrepareOrderCommand(service, lastOrder));
                        invoker.Invoke();
                        
                        invoker.SetCommand(new MarkReadyCommand(service, lastOrder));
                        invoker.Invoke();
                        
                        Console.WriteLine("[Workflow] Order is now ready for serving!");
                    }
                    else Console.WriteLine("No last order.");
                    break;

                default:
                    Console.WriteLine("Unknown option.");
                    break;
            }
        }
    }
}
