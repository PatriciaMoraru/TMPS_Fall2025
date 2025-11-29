using System;

namespace KitchenOrderApp.Patterns.Observer;

public class KitchenDisplayObserver
{
    public void Update(object sender, OrderStatusChangedEventArgs e)
    {
        Console.WriteLine($"[Kitchen Display] Order {e.Order.Id} status: {e.Order.Status}.");
    }
}

