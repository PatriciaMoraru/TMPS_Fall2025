using StructuralPatterns.Decorator;

namespace StructuralPatterns.Composite;

/// <summary>
/// Leaf node in the Composite pattern: wraps a single IGift,
/// which may itself be decorated (wrapping, card, discount, etc.).
/// </summary>
public class GiftItemLeaf : GiftComponent
{
    private IGift _gift;

    public GiftItemLeaf(IGift gift)
    {
        _gift = gift;
    }

    public override string Name => _gift.Name;

    /// <summary>
    /// Exposes the underlying gift so we can wrap it with decorators later.
    /// </summary>
    public IGift Gift
    {
        get => _gift;
        set => _gift = value;
    }

    public override decimal GetPrice() => _gift.GetPrice();
}