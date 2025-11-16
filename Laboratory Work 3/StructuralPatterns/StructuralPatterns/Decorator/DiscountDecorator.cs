namespace StructuralPatterns.Decorator;

/// <summary>
/// Applies a percentage discount to the gift price.
/// </summary>
public class DiscountDecorator : GiftDecorator
{
    private readonly decimal _discountFactor;

    /// <param name="discountFactor">e.g. 0.9m for 10% off.</param>
    public DiscountDecorator(IGift inner, decimal discountFactor)
        : base(inner)
    {
        _discountFactor = discountFactor;
    }

    public override decimal GetPrice() =>
        Inner.GetPrice() * _discountFactor;

    public override string Describe() =>
        Inner.Describe() + $" (discount applied)";
}