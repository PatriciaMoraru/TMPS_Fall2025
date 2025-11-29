using KitchenOrderApp.Domain;
using KitchenOrderApp.Services;

namespace KitchenOrderApp.Patterns.Command;

public class PrepareOrderCommand : Command
{
    public PrepareOrderCommand(OrderService service, Order order)
        : base(service, order)
    {
    }

    public override void Execute()
    {
        service.ChangeStatus(order, OrderStatus.InPreparation);
    }
}

