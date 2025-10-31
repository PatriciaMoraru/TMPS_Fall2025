using Bakery.Domain;

namespace Bakery.Factory;

public interface ICakeBuilder
{
    void Reset();
    void SetBaseLayer(string baseLayer);
    void AddCream(string cream);
    void AddDecoration(string decoration);
    void SetPrice(decimal price);
    CustomCake Build();
}