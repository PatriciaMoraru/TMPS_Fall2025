using System.Collections.Generic;
using System.Linq;
using KitchenOrderApp.Domain;

namespace KitchenOrderApp.Services;

public class OrderRepository
{
    private readonly List<Order> _orders = new();
    private int _nextId = 1;

    public Order CreateOrder(int tableNumber)
    {
        var order = new Order(_nextId++, tableNumber);
        _orders.Add(order);
        return order;
    }

    public Order? GetById(int id) => _orders.FirstOrDefault(o => o.Id == id);

    public IEnumerable<Order> GetAll() => _orders;
}

