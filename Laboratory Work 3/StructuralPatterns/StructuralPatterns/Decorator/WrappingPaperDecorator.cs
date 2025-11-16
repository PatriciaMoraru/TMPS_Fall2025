namespace StructuralPatterns.Decorator;

/// <summary>
/// Adds wrapping paper to a gift and increases its price.
/// </summary>
public class WrappingPaperDecorator : GiftDecorator
{
    private readonly decimal _wrapPrice;

    public WrappingPaperDecorator(IGift inner, decimal wrapPrice = 20m)
        : base(inner)
    {
        _wrapPrice = wrapPrice;
    }

    public override decimal GetPrice() => Inner.GetPrice() + _wrapPrice;

    public override string Describe() =>
        Inner.Describe() + " + decorative wrapping paper";
}