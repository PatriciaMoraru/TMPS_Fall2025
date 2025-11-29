using System;
using System.Linq;
using KitchenOrderApp.Domain;

namespace KitchenOrderApp.Patterns.ChainOfResponsibility;

public class DrinkStationHandler : Handler
{
    public override void Handle(Order order)
    {
        if (order.Items.Any(i => i.Category == ItemCategory.Drink))
        {
            Console.WriteLine($"[Drink Station] Preparing drinks for order {order.Id}.");
        }

        successor?.Handle(order);
    }
}

