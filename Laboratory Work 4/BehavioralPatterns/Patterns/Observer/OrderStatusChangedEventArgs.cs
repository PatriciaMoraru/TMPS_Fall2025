using System;
using KitchenOrderApp.Domain;

namespace KitchenOrderApp.Patterns.Observer;

public class OrderStatusChangedEventArgs : EventArgs
{
    public Order Order { get; }

    public OrderStatusChangedEventArgs(Order order)
    {
        Order = order;
    }
}

