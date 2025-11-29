using System.Collections.Generic;

namespace KitchenOrderApp.Domain;

public class Order
{
    public int Id { get; }
    public int TableNumber { get; }
    public List<OrderItem> Items { get; } = new();
    public OrderStatus Status { get; set; } = OrderStatus.New;

    public Order(int id, int tableNumber)
    {
        Id = id;
        TableNumber = tableNumber;
    }

    public override string ToString()
        => $"Order {Id} | Table {TableNumber} | Status: {Status} | Items: {Items.Count}";
}

