using KitchenOrderApp.Domain;
using KitchenOrderApp.Services;

namespace KitchenOrderApp.Patterns.Command;

public abstract class Command
{
    protected readonly OrderService service;
    protected readonly Order order;

    protected Command(OrderService service, Order order)
    {
        this.service = service;
        this.order = order;
    }

    public abstract void Execute();
}

