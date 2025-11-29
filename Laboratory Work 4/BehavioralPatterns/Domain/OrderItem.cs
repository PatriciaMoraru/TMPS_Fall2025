namespace KitchenOrderApp.Domain;

public class OrderItem
{
    public string Name { get; }
    public ItemCategory Category { get; }
    public int Quantity { get; }

    public OrderItem(string name, ItemCategory category, int quantity)
    {
        Name = name;
        Category = category;
        Quantity = quantity;
    }

    public override string ToString()
        => $"{Quantity} x {Name} ({Category})";
}

