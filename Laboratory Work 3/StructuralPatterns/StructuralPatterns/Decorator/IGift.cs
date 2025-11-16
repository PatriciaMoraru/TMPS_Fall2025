namespace StructuralPatterns.Decorator;

/// <summary>
/// Interface for a gift item that has a name, price and description.
/// This is what our decorators will wrap.
/// </summary>
public interface IGift
{
    string Name { get; }
    decimal GetPrice();
    string Describe();
}