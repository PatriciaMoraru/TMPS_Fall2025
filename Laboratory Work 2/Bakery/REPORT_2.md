# Creational Design Patterns - Bakery

## Author: Patricia Moraru

---

## Objectives

* Get familiar with the Creational Design Patterns (CDPs);
* Choose a specific domain and model it using multiple patterns;
* Implement at least 3 CDPs for the selected domain and demonstrate their behavior in a single C# application.

---

## Domain: Bakery Management System

The chosen domain is a **Bakery**, where products such as breads, cakes, and croissants are prepared using various creation mechanisms.  
Each creational design pattern is mapped to a real bakery scenario: producing, cloning, building, or pooling resources for products.

---

## Used Design Patterns

* Singleton
* Builder
* Prototype
* Object Pool
* Factory Method
* Abstract Factory

---

## Implementation

### Overview

The project simulates a bakery system that demonstrates six creational design patterns.  
Each pattern is implemented independently, with a shared `BakeryLogger` Singleton used across all modules.  
The `Program.cs` file provides a menu that allows the user to test each pattern separately.

---

### 1️. **Singleton Pattern – BakeryLogger**

The `BakeryLogger` ensures that all baking operations share the same logging instance throughout the system.

```csharp
public sealed class BakeryLogger
{
    private static BakeryLogger _instance;
    private static readonly object _lock = new object();
    private BakeryLogger() { }

    public static BakeryLogger Instance
    {
        get
        {
            lock (_lock)
            {
                return _instance ??= new BakeryLogger();
            }
        }
    }

    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {message}");
    }
}
```

Used implicitly by all domain classes:
```csharp
BakeryLogger.Instance.Log("Baking Sweet Croissant...");
```

---

### 2. **Builder Pattern – Custom Cake Builder**

This pattern constructs complex cake objects step-by-step using a builder and a director.

```csharp
public interface ICakeBuilder
{
    void SetBaseLayer(string layer);
    void AddCream(string cream);
    void AddDecoration(string decoration);
    void SetPrice(double price);
    CustomCake Build();
}

public class CustomCakeBuilder : ICakeBuilder
{
    private CustomCake _cake = new CustomCake();
    public void SetBaseLayer(string layer) => _cake.BaseLayer = layer;
    public void AddCream(string cream) => _cake.Cream = cream;
    public void AddDecoration(string decoration) => _cake.Decoration = decoration;
    public void SetPrice(double price) => _cake.Price = price;

    public CustomCake Build()
    {
        var result = _cake;
        _cake = new CustomCake();
        return result;
    }
}
```

**Director:**
```csharp
public class CakeDirector
{
    private readonly ICakeBuilder _builder;
    public CakeDirector(ICakeBuilder builder) => _builder = builder;

    public CustomCake BuildBirthdayCake()
    {
        _builder.SetBaseLayer("Vanilla");
        _builder.AddCream("Chocolate");
        _builder.AddDecoration("Candles");
        _builder.SetPrice(25.0);
        return _builder.Build();
    }
}
```

---

### 3. **Prototype Pattern – Recipe Cloning**

Used to duplicate recipe templates for similar pastries with minor differences.

```csharp
public class Recipe : ICloneable
{
    public string Name { get; set; }
    public List<string> Ingredients { get; set; }

    public Recipe(string name, params string[] ingredients)
    {
        Name = name;
        Ingredients = ingredients.ToList();
    }

    public object Clone()
    {
        return new Recipe(Name, Ingredients.ToArray());
    }

    public void Show()
    {
        Console.WriteLine($"Recipe: {Name}");
        Console.WriteLine($"Ingredients: {string.Join(", ", Ingredients)}\n");
    }
}
```

**Output Example:**
```
--- PROTOTYPE PATTERN ---
Cloning recipe for Chocolate Croissant...
Recipe: Chocolate Croissant
Ingredients: Flour, Butter, Chocolate
Recipe: Almond Croissant
Ingredients: Flour, Butter, Chocolate, Almonds
```

---

### 4. **Object Pool Pattern – MachineServicePool**

Simulates reusing expensive resources like bakery machines instead of recreating them.

```csharp
public class MachineServicePool
{
    private readonly Queue<MachineService> _available = new();
    private readonly List<MachineService> _inUse = new();

    public MachineServicePool(int count)
    {
        for (int i = 0; i < count; i++)
            _available.Enqueue(new MachineService());
    }

    public MachineService Acquire()
    {
        var machine = _available.Count > 0 ? _available.Dequeue() : new MachineService();
        _inUse.Add(machine);
        Console.WriteLine("Machine acquired from pool.");
        return machine;
    }

    public void Release(MachineService machine)
    {
        _inUse.Remove(machine);
        _available.Enqueue(machine);
        Console.WriteLine("Machine returned to pool.");
    }
}
```

**Demo Output:**
```
--- OBJECT POOL PATTERN ---
Machine acquired from pool.
Machine acquired from pool.
Machine returned to pool.
Object Pool demonstration complete.
```

---

### 5. **Factory Method Pattern – Pastry Factory**

Creates pastries (Croissant, Muffin) through subclasses that override a common factory method.

```csharp
public abstract class PastryFactory
{
    public abstract IPastry CreatePastry();

    public void BakePastry()
    {
        var pastry = CreatePastry();
        pastry.Bake();
    }
}

public class CroissantFactoryMethod : PastryFactory
{
    public override IPastry CreatePastry() => new CroissantPastry();
}

public class MuffinFactoryMethod : PastryFactory
{
    public override IPastry CreatePastry() => new MuffinPastry();
}
```

**Output Example:**
```
--- FACTORY METHOD PATTERN ---
Baking a Croissant pastry...
Baking a Muffin pastry...
```

---

### 6. **Abstract Factory Pattern – Sweet vs Chocolate Families**

Defines factories for related product families: cakes, breads, and croissants.

```csharp
public interface IBakeryFactory
{
    Croissant CreateCroissant(MachineService service);
    Bread CreateBread(MachineService service);
    Cake CreateCake(MachineService service);
}

public class SweetBakeryFactory : IBakeryFactory
{
    public Croissant CreateCroissant(MachineService service) => new SweetCroissant(service);
    public Bread CreateBread(MachineService service) => new SweetBread(service);
    public Cake CreateCake(MachineService service) => new SweetCake(service);
}

public class ChocolateBakeryFactory : IBakeryFactory
{
    public Croissant CreateCroissant(MachineService service) => new ChocolateCroissant(service);
    public Bread CreateBread(MachineService service) => new ChocolateBread(service);
    public Cake CreateCake(MachineService service) => new ChocolateCake(service);
}
```

**Output Example:**
```
--- ABSTRACT FACTORY PATTERN ---
Creating sweet family of bakery products...
Baking Sweet Croissant...
Baking Sweet Bread...
Baking Sweet Cake...
```

---

## Conclusion
The Bakery Management System effectively illustrates all six creational design patterns in a simple and intuitive way.  
Each pattern is modular, easy to understand, and grounded in a realistic domain — making the code both educational and extendable.
---
