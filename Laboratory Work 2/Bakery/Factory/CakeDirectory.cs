using Bakery.Domain;

namespace Bakery.Factory;

public class CakeDirector
{
    public CustomCake BuildChocolateCake(ICakeBuilder builder)
    {
        builder.Reset();
        builder.SetBaseLayer("Chocolate Sponge");
        builder.AddCream("Chocolate Ganache");
        builder.AddDecoration("Choco Shavings");
        builder.SetPrice(20m);
        return builder.Build();
    }

    public CustomCake BuildBirthdayCake(ICakeBuilder builder)
    {
        builder.Reset();
        builder.SetBaseLayer("Vanilla + Strawberry");
        builder.AddCream("Buttercream");
        builder.AddDecoration("Candles & Confetti");
        builder.SetPrice(25m);
        return builder.Build();
    }
}