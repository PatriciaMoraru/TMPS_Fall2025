using System;
using KitchenOrderApp.Domain;

namespace KitchenOrderApp.Patterns.Observer;

public class WaiterObserver
{
    // Observer method
    public void Update(object sender, OrderStatusChangedEventArgs e)
    {
        var order = e.Order;
        if (order.Status == OrderStatus.Ready)
        {
            Console.WriteLine($"[Waiter] Order {order.Id} for table {order.TableNumber} is READY!");
        }
        else
        {
            Console.WriteLine($"[Waiter] Order {order.Id} changed status to {order.Status}.");
        }
    }
}

