using System;
using KitchenOrderApp.Domain;
using KitchenOrderApp.Patterns.ChainOfResponsibility;
using KitchenOrderApp.Patterns.Observer;

namespace KitchenOrderApp.Services;

public class OrderService
{
    private readonly OrderRepository repository;
    private readonly OrderNotifier notifier;
    private readonly Handler kitchenChain;

    public OrderService(OrderRepository repository, OrderNotifier notifier)
    {
        this.repository = repository;
        this.notifier = notifier;

        // Build the GoF CoR chain
        var drink = new DrinkStationHandler();
        var grill = new GrillStationHandler();
        var dessert = new DessertStationHandler();

        drink.SetSuccessor(grill);
        grill.SetSuccessor(dessert);

        kitchenChain = drink;
    }

    public Order CreateOrder(int tableNumber)
    {
        var order = repository.CreateOrder(tableNumber);
        Console.WriteLine($"[System] Created {order}");
        return order;
    }

    public void SendToKitchen(Order order)
    {
        Console.WriteLine($"[System] Sending order {order.Id} to kitchen chain.");
        kitchenChain.Handle(order);
    }

    public void ChangeStatus(Order order, OrderStatus newStatus)
    {
        Console.WriteLine($"[System] Changing status of order {order.Id} from {order.Status} to {newStatus}.");
        order.Status = newStatus;
        notifier.NotifyStatusChanged(order);
    }

    public void PrintOrders()
    {
        foreach (var o in repository.GetAll())
        {
            Console.WriteLine(o);
        }
    }
}

