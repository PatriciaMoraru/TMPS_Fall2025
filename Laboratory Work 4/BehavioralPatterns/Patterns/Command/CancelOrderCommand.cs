using KitchenOrderApp.Domain;
using KitchenOrderApp.Services;

namespace KitchenOrderApp.Patterns.Command;

public class CancelOrderCommand : Command
{
    public CancelOrderCommand(OrderService service, Order order)
        : base(service, order)
    {
    }

    public override void Execute()
    {
        service.ChangeStatus(order, OrderStatus.Cancelled);
    }
}

