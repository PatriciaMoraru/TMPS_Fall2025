using KitchenOrderApp.Domain;

namespace KitchenOrderApp.Patterns.Observer;

public delegate void OrderStatusChangedHandler(object sender, OrderStatusChangedEventArgs e);

public class OrderNotifier
{
    // Subject in GoF Observer
    public event OrderStatusChangedHandler? StatusChanged;

    public void NotifyStatusChanged(Order order)
    {
        StatusChanged?.Invoke(this, new OrderStatusChangedEventArgs(order));
    }
}

