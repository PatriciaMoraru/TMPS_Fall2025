using Bakery.Domain;

namespace Bakery.Factory;

public class CustomCakeBuilder : ICakeBuilder
{
    private CustomCake _cake = new();

    public void Reset()
    {
        _cake = new CustomCake();
    }

    public void SetBaseLayer(string baseLayer)
    {
        _cake.BaseLayer = baseLayer;
    }

    public void AddCream(string cream)
    {
        _cake.Cream = cream;
    }

    public void AddDecoration(string decoration)
    {
        _cake.Decoration = decoration;
    }

    public void SetPrice(decimal price)
    {
        _cake.Price = price;
    }

    public CustomCake Build()
    {
        var result = _cake;
        Reset();
        return result;
    }
}