# Structural Design Patterns

**Author:** Patricia Moraru
**Course:** TMPS (Techniques and Mechanisms of Software Design)  
**Laboratory Work:** #3 - Structural Design Patterns

---

## Objectives

1. Study and understand the Structural Design Patterns
2. Choose a specific domain for implementation
3. Implement at least 3 Structural Design Patterns for the chosen domain
4. Demonstrate the integration and interaction between multiple patterns

---

## Domain

The chosen domain is a **Gift Shop Management System**. This system manages a catalog of gift items and gift boxes, allowing customers to view products, check prices, and see detailed information about decorated and bundled gifts.

The domain naturally fits structural patterns because:
- Gifts can be composed into boxes (Composite pattern)
- Gifts can be decorated with wrapping, ribbons, cards, and discounts (Decorator pattern)
- The system needs a simplified interface for complex operations (Facade pattern)
- Some gifts require expensive data loading from external sources (Proxy pattern)

---

## Used Design Patterns

### 1. **Composite Pattern**
Allows treating individual gift items and gift boxes uniformly as a tree structure, enabling recursive price calculation and hierarchical organization.

### 2. **Decorator Pattern**
Dynamically adds responsibilities to gift objects (wrapping paper, ribbons, personalized cards, discounts) without modifying their structure.

### 3. **Facade Pattern**
Provides a simplified interface for interacting with the complex subsystem of composites and decorators, hiding implementation details from the client.

### 4. **Proxy Pattern**
Controls access to expensive gift objects, implementing lazy loading for gifts that require external API calls or database queries.

---

## Implementation

### 1. Composite Pattern

The Composite pattern is implemented through a component hierarchy that allows building tree structures of gifts. The `GiftComponent` abstract class serves as the base, with `GiftBoxComposite` representing containers and `GiftItemLeaf` representing individual items.

**Key Classes:**

**GiftComponent.cs** - Base component defining the interface:
```csharp
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
```

**GiftBoxComposite.cs** - Composite implementation:
```csharp
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
```

**GiftItemLeaf.cs** - Leaf implementation:
```csharp
public class GiftItemLeaf : GiftComponent
{
    private IGift _gift;

    public GiftItemLeaf(IGift gift)
    {
        _gift = gift;
    }

    public override string Name => _gift.Name;

    public IGift Gift
    {
        get => _gift;
        set => _gift = value;
    }

    public override decimal GetPrice() => _gift.GetPrice();
}
```

This pattern enables creating hierarchical structures like:
```
Gift Catalog
├── Birthday Box
│   ├── Chocolate Gift
│   └── Mug Gift
└── Christmas Box
    ├── Coffee Set
    ├── Socks
    └── Mystery Box
```

---

### 2. Decorator Pattern

The Decorator pattern adds functionality to gifts dynamically by wrapping them in decorator objects. Each decorator adds a specific feature (wrapping, ribbon, card, discount) while maintaining the same interface.

**IGift.cs** - Component interface:
```csharp
public interface IGift
{
    string Name { get; }
    decimal GetPrice();
    string Describe();
}
```

**BasicGift.cs** - Concrete component:
```csharp
public class BasicGift : IGift
{
    public string Name { get; }
    private readonly decimal _basePrice;

    public BasicGift(string name, decimal basePrice)
    {
        Name = name;
        _basePrice = basePrice;
    }

    public decimal GetPrice() => _basePrice;

    public string Describe() => Name;
}
```

**GiftDecorator.cs** - Base decorator:
```csharp
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
```

**Concrete Decorators:**

**WrappingPaperDecorator.cs:**
```csharp
public class WrappingPaperDecorator : GiftDecorator
{
    private readonly decimal _wrapPrice;

    public WrappingPaperDecorator(IGift inner, decimal wrapPrice = 20m)
        : base(inner)
    {
        _wrapPrice = wrapPrice;
    }

    public override decimal GetPrice() => Inner.GetPrice() + _wrapPrice;

    public override string Describe() =>
        Inner.Describe() + " + decorative wrapping paper";
}
```

**RibbonDecorator.cs:**
```csharp
public class RibbonDecorator : GiftDecorator
{
    private readonly decimal _ribbonPrice;

    public RibbonDecorator(IGift inner, decimal ribbonPrice = 10m)
        : base(inner)
    {
        _ribbonPrice = ribbonPrice;
    }

    public override decimal GetPrice() => Inner.GetPrice() + _ribbonPrice;

    public override string Describe() =>
        Inner.Describe() + " + ribbon";
}
```

**PersonalizedCardDecorator.cs:**
```csharp
public class PersonalizedCardDecorator : GiftDecorator
{
    private readonly decimal _cardPrice;
    private readonly string _message;

    public PersonalizedCardDecorator(IGift inner, string message, decimal cardPrice = 15m)
        : base(inner)
    {
        _message = message;
        _cardPrice = cardPrice;
    }

    public override decimal GetPrice() => Inner.GetPrice() + _cardPrice;

    public override string Describe() =>
        Inner.Describe() + $" + card with message: \"{_message}\"";
}
```

**DiscountDecorator.cs:**
```csharp
public class DiscountDecorator : GiftDecorator
{
    private readonly decimal _discountFactor;

    public DiscountDecorator(IGift inner, decimal discountFactor)
        : base(inner)
    {
        _discountFactor = discountFactor;
    }

    public override decimal GetPrice() =>
        Inner.GetPrice() * _discountFactor;

    public override string Describe() =>
        Inner.Describe() + $" (discount applied)";
}
```

**Usage Example:**
```csharp
// Building a decorated gift with multiple layers
IGift gift = new BasicGift("Premium Chocolate Box", 150m);
gift = new WrappingPaperDecorator(gift, 20m);
gift = new RibbonDecorator(gift, 10m);
gift = new PersonalizedCardDecorator(gift, "Happy Birthday!");

// Result: Premium Chocolate Box + decorative wrapping paper + ribbon 
//         + card with message: "Happy Birthday!" (180 MDL)
```

---

### 3. Facade Pattern

The Facade pattern simplifies the interaction with the complex composite and decorator structures. It provides high-level methods for common operations like showing the catalog, getting prices, and displaying detailed information.

**GiftShopFacade.cs:**
```csharp
public class GiftShopFacade
{
    private readonly GiftBoxComposite _catalogRoot;

    public GiftShopFacade(GiftBoxComposite catalogRoot)
    {
        _catalogRoot = catalogRoot;
    }

    public void ShowCatalog()
    {
        Console.WriteLine("=== Gift Catalog ===");
        PrintTree(_catalogRoot, 0);
        Console.WriteLine();
    }

    public decimal GetBoxPrice(string boxName)
    {
        var comp = FindComponentByName(_catalogRoot, boxName);
        if (comp is null)
        {
            Console.WriteLine($"Gift box '{boxName}' not found.");
            return 0m;
        }

        var price = comp.GetPrice();
        Console.WriteLine($"Total price of '{boxName}': {price} MDL");
        return price;
    }

    public void ShowBoxDetails(string boxName)
    {
        var comp = FindComponentByName(_catalogRoot, boxName);
        if (comp is null)
        {
            Console.WriteLine($"Gift box '{boxName}' not found.");
            return;
        }

        Console.WriteLine($"=== Details for '{boxName}' ===");
        PrintBoxDetails(comp, 0);
        Console.WriteLine();
    }

    private GiftComponent? FindComponentByName(GiftComponent root, string name)
    {
        if (root.Name == name) return root;

        foreach (var child in root.GetChildren())
        {
            var found = FindComponentByName(child, name);
            if (found != null) return found;
        }

        return null;
    }

    private void PrintTree(GiftComponent root, int indentLevel)
    {
        var indent = new string(' ', indentLevel * 2);
        Console.WriteLine($"{indent}- {root.Name} ({root.GetPrice()} MDL)");

        foreach (var child in root.GetChildren())
        {
            PrintTree(child, indentLevel + 1);
        }
    }

    private void PrintBoxDetails(GiftComponent component, int indentLevel)
    {
        var indent = new string(' ', indentLevel * 2);

        if (component is GiftItemLeaf leaf)
        {
            var desc = leaf.Gift.Describe();
            var price = leaf.GetPrice();
            Console.WriteLine($"{indent}- {desc} [{price} MDL]");
        }
        else
        {
            Console.WriteLine($"{indent}* {component.Name} ({component.GetPrice()} MDL)");

            foreach (var child in component.GetChildren())
            {
                PrintBoxDetails(child, indentLevel + 1);
            }
        }
    }
}
```

The facade hides the complexity of:
- Recursive tree traversal
- Type checking (composite vs leaf)
- Decorator unwrapping
- Price aggregation

**Client Code (Program.cs):**
```csharp
static void Main(string[] args)
{
    // Build demo catalog (Composite + Decorator)
    var catalogRoot = GiftShopBootstrapper.CreateDemoCatalog();

    // Facade for interacting with the system
    var facade = new GiftShopFacade(catalogRoot);

    RunMenu(facade);
}
```

---

### 4. Proxy Pattern

The Proxy pattern is implemented as a Virtual Proxy that delays the creation of expensive gift objects until they are actually needed. This is particularly useful for gifts that require external API calls or database queries.

**GiftProxy.cs:**
```csharp
public class GiftProxy : IGift
{
    private readonly string _displayName;
    private readonly Func<IGift> _loader;
    private IGift? _realGift;

    public GiftProxy(string displayName, Func<IGift> loader)
    {
        _displayName = displayName;
        _loader = loader;
    }

    public string Name => _displayName;

    private IGift RealGift
    {
        get
        {
            if (_realGift == null)
            {
                Console.WriteLine($"[Proxy] Loading real gift data for '{_displayName}'...");
                _realGift = _loader();
            }

            return _realGift;
        }
    }

    public decimal GetPrice() => RealGift.GetPrice();

    public string Describe() => RealGift.Describe();
}
```

**Usage Example:**
```csharp
// Gift from external partner, loaded lazily via Proxy
IGift mysteryProxy = new GiftProxy("Mystery Partner Box", () =>
{
    // This code runs ONLY when price/description is actually needed
    Console.WriteLine("[Proxy] Fetching details from external partner shop...");

    IGift real = new BasicGift("Luxury Mystery Box (Partner Shop)", 300m);
    real = new WrappingPaperDecorator(real, 30m);
    real = new RibbonDecorator(real, 15m);
    real = new PersonalizedCardDecorator(real, "Surprise inside!", 15m);

    return real;
});

var mysteryLeaf = new GiftItemLeaf(mysteryProxy);
christmasBox.Add(mysteryLeaf);
```

The proxy provides:
- **Lazy initialization**: The real gift is created only when accessed
- **Caching**: Once loaded, the real gift is stored for subsequent calls
- **Transparency**: Works seamlessly with decorators and composites
- **Performance**: Avoids expensive operations until necessary

---

## Pattern Integration

One of the key achievements of this implementation is how the patterns work together seamlessly:

```
Client
  ↓
Facade (simplified interface)
  ↓
Composite (tree structure)
  ├─ GiftBoxComposite
  │   ├─ GiftItemLeaf
  │   │   └─ Decorator Chain
  │   │       └─ Proxy (lazy loading)
  │   │           └─ BasicGift
  │   └─ GiftItemLeaf
  │       └─ Decorator Chain
  │           └─ BasicGift
  └─ GiftBoxComposite
      └─ ...
```

**Integration Flow:**
1. **Proxy** wraps expensive-to-create `BasicGift` objects
2. **Decorators** wrap both proxies and basic gifts to add features
3. **Composite** organizes decorated gifts into hierarchies
4. **Facade** provides a simple API to navigate and display the entire structure

---

## System Architecture

```
┌─────────────────────────────────────────────────┐
│                  Program.cs                     │
│             (Client / Entry Point)              │
└────────────────────┬────────────────────────────┘
                     │
                     ▼
         ┌───────────────────────┐
         │   GiftShopFacade      │  ◄─── Facade Pattern
         │  (Simplified API)     │
         └───────────┬───────────┘
                     │
                     ▼
         ┌───────────────────────┐
         │  GiftBoxComposite     │  ◄─── Composite Pattern
         │   (Tree Structure)    │
         └───────────┬───────────┘
                     │
          ┌──────────┴──────────┐
          ▼                     ▼
    ┌─────────┐          ┌─────────┐
    │  Leaf   │          │  Leaf   │
    └────┬────┘          └────┬────┘
         │                    │
         ▼                    ▼
    ┌─────────────┐      ┌─────────────┐
    │ Decorators  │      │   Proxy     │  ◄─── Proxy Pattern
    │  (Chain)    │      │ (Lazy Load) │
    └──────┬──────┘      └──────┬──────┘
           │                    │
           ▼                    ▼
    ┌──────────────────────────────┐
    │        BasicGift             │  ◄─── Decorator Pattern
    │    (Concrete Component)      │
    └──────────────────────────────┘
```

---

## Results / Screenshots

### Menu Interface:
```
==================================
         GIFT SHOP MENU
==================================
1. Show full catalog
2. Show price of a gift box
3. Show detailed contents of a gift box
4. Exit
==================================
Choose an option (1-4):
```

### Option 1 - Full Catalog:
```
>>> Catalog

=== Gift Catalog ===
[Proxy] Loading real gift data for 'Mystery Partner Box'...
[Proxy] Fetching details from external partner shop...
- Gift Catalog (982,25 MDL)
  - Birthday Box (308,25 MDL)
    - Premium Chocolate Box (180 MDL)
    - Cute Birthday Mug (128,25 MDL)
  - Christmas Box (674,0 MDL)
    - Christmas Coffee Set (250 MDL)
    - Funny Christmas Socks (64,0 MDL)
    - Mystery Partner Box (360 MDL)
```

The output shows:
- **Composite**: Hierarchical tree structure with boxes and items
- **Decorator**: Prices include all decorations (wrapping, ribbons, cards)
- **Proxy**: Lazy loading message when accessing expensive gift data

### Option 2 - Price Query:
```
Enter the exact name of the gift box: Birthday Box

Total price of 'Birthday Box': 308,25 MDL
```

### Option 3 - Detailed Contents:
```
Enter the exact name of the gift box: Christmas Box

=== Details for 'Christmas Box' ===
* Christmas Box (674,0 MDL)
  - Christmas Coffee Set + decorative wrapping paper + ribbon 
    + card with message: "Merry Christmas!" [250 MDL]
  - Funny Christmas Socks (discount applied) [64,0 MDL]
  - Luxury Mystery Box (Partner Shop) + decorative wrapping paper 
    + ribbon + card with message: "Surprise inside!" [360 MDL]
```

This demonstrates:
- **Decorator**: Full description showing all applied decorations
- **Composite**: Recursive display of box contents
- **Proxy**: The mystery box details are displayed from cached data

---

## Conclusions

This laboratory work successfully demonstrates the implementation and integration of four structural design patterns in a cohesive gift shop domain:

1. **Composite Pattern** enables treating individual gifts and gift boxes uniformly, allowing recursive operations like price calculation across hierarchical structures. This pattern is essential for representing part-whole hierarchies.

2. **Decorator Pattern** provides flexible functionality extension by allowing gifts to be dynamically wrapped with additional features (wrapping paper, ribbons, cards, discounts). Multiple decorators can be stacked without modifying the original gift classes.

3. **Facade Pattern** significantly simplifies client interaction by hiding the complexity of navigating composite structures and working with decorated objects. It provides a clean, intuitive API for common operations.

4. **Proxy Pattern** demonstrates performance optimization through lazy loading of expensive resources. The virtual proxy delays object creation until necessary, which is crucial for gifts requiring external API calls or database queries.

### Key Learnings:

- **Pattern Synergy**: The patterns work together naturally, with each solving a specific problem while complementing the others. The combination is more powerful than individual patterns.

- **Flexibility**: The system is highly extensible. New types of decorators, gift items, or composite structures can be added without modifying existing code.

- **Real-World Applicability**: These patterns are not just academic exercises—they solve real problems in software design like managing complexity, enabling extension, and optimizing performance.

- **Code Maintainability**: Structural patterns significantly improve code organization and maintainability by clearly separating concerns and responsibilities.
