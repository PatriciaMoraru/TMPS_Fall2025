namespace StructuralPatterns.Composite;

/// <summary>
/// Base component in the Composite pattern.
/// Represents either a single gift item or a gift box containing others.
/// </summary>
public abstract class GiftComponent
{
    public abstract string Name { get; }

    public virtual decimal GetPrice() => 0m;

    public virtual void Add(GiftComponent component)
        => throw new NotSupportedException($"{GetType().Name} cannot contain children.");

    public virtual void Remove(GiftComponent component)
        => throw new NotSupportedException($"{GetType().Name} cannot contain children.");

    public virtual IEnumerable<GiftComponent> GetChildren()
        => Enumerable.Empty<GiftComponent>();
}