namespace StructuralPatterns.Decorator;

/// <summary>
/// Adds a ribbon to the gift.
/// </summary>
public class RibbonDecorator : GiftDecorator
{
    private readonly decimal _ribbonPrice;

    public RibbonDecorator(IGift inner, decimal ribbonPrice = 10m)
        : base(inner)
    {
        _ribbonPrice = ribbonPrice;
    }

    public override decimal GetPrice() => Inner.GetPrice() + _ribbonPrice;

    public override string Describe() =>
        Inner.Describe() + " + ribbon";
}