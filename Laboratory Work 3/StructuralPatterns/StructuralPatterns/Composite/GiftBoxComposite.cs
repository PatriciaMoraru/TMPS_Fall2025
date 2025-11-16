namespace StructuralPatterns.Composite;

/// <summary>
/// Composite node representing a gift box that can contain other gifts or boxes.
/// </summary>
public class GiftBoxComposite : GiftComponent
{
    private readonly List<GiftComponent> _children = new();

    public override string Name { get; }

    public GiftBoxComposite(string name)
    {
        Name = name;
    }

    public override void Add(GiftComponent component) => _children.Add(component);

    public override void Remove(GiftComponent component) => _children.Remove(component);

    public override IEnumerable<GiftComponent> GetChildren() => _children;

    public override decimal GetPrice() =>
        _children.Sum(c => c.GetPrice());
}