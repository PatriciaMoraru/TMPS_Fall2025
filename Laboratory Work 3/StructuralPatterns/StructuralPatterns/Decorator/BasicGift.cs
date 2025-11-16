namespace StructuralPatterns.Decorator;

/// <summary>
/// Simple concrete gift, e.g. "Chocolate Box" or "Coffee Mug".
/// </summary>
public class BasicGift : IGift
{
    public string Name { get; }
    private readonly decimal _basePrice;

    public BasicGift(string name, decimal basePrice)
    {
        Name = name;
        _basePrice = basePrice;
    }

    public decimal GetPrice() => _basePrice;

    public string Describe() => Name;
}