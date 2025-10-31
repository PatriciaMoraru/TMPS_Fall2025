using System;

namespace Bakery.Domain;
public class Recipe : ICloneable
{
    public string Name { get; set; }
    public string Ingredients { get; set; }

    public Recipe(string name, string ingredients)
    {
        Name = name;
        Ingredients = ingredients;
    }

    public object Clone()
    {
        Console.WriteLine($"Cloning recipe for {Name}...");
        return new Recipe(Name, Ingredients);
    }

    public void Display()
    {
        Console.WriteLine($"Recipe: {Name}");
        Console.WriteLine($"Ingredients: {Ingredients}\n");
    }
}