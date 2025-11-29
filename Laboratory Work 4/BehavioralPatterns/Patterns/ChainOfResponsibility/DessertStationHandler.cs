using System;
using System.Linq;
using KitchenOrderApp.Domain;

namespace KitchenOrderApp.Patterns.ChainOfResponsibility;

public class DessertStationHandler : Handler
{
    public override void Handle(Order order)
    {
        if (order.Items.Any(i => i.Category == ItemCategory.Dessert))
        {
            Console.WriteLine($"[Dessert Station] Preparing desserts for order {order.Id}.");
        }

        successor?.Handle(order);
    }
}

