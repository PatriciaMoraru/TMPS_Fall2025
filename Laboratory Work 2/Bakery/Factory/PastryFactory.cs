using System;

namespace Bakery.Factory;
public abstract class PastryFactory
{
    // Factory Method
    public abstract Pastry CreatePastry();

    public void BakePastry()
    {
        var pastry = CreatePastry();
        Console.WriteLine($"Preparing a {pastry.Name}...");
        pastry.Bake();
    }
}

public class CroissantFactoryMethod : PastryFactory
{
    public override Pastry CreatePastry()
    {
        return new CroissantPastry();
    }
}

public class MuffinFactoryMethod : PastryFactory
{
    public override Pastry CreatePastry()
    {
        return new MuffinPastry();
    }
}

public abstract class Pastry
{
    public abstract string Name { get; }
    public abstract void Bake();
}

public class CroissantPastry : Pastry
{
    public override string Name => "Croissant";
    public override void Bake() => Console.WriteLine("Baking a buttery croissant...\n");
}

public class MuffinPastry : Pastry
{
    public override string Name => "Muffin";
    public override void Bake() => Console.WriteLine("Baking a fluffy muffin...\n");
}