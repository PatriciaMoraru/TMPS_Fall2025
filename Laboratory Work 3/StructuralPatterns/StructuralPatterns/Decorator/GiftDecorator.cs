namespace StructuralPatterns.Decorator;

/// <summary>
/// Base decorator that wraps an IGift and forwards calls by default.
/// </summary>
public abstract class GiftDecorator : IGift
{
    protected readonly IGift Inner;

    protected GiftDecorator(IGift inner)
    {
        Inner = inner;
    }

    public virtual string Name => Inner.Name;

    public virtual decimal GetPrice() => Inner.GetPrice();

    public virtual string Describe() => Inner.Describe();
}