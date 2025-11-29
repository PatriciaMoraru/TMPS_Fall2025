using System;
using System.Linq;
using KitchenOrderApp.Domain;

namespace KitchenOrderApp.Patterns.ChainOfResponsibility;

public class GrillStationHandler : Handler
{
    public override void Handle(Order order)
    {
        if (order.Items.Any(i => i.Category == ItemCategory.Grill))
        {
            Console.WriteLine($"[Grill Station] Grilling items for order {order.Id}.");
        }

        successor?.Handle(order);
    }
}

