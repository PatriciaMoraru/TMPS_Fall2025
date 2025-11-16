using System;
using StructuralPatterns.Composite;

namespace StructuralPatterns.Facade;

/// <summary>
/// Facade that provides a simple API for interacting with the gift shop.
/// Client code should only talk to this class.
/// </summary>
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

    /// <summary>
    /// Shows detailed contents of a specific box:
    /// for each decorated gift, prints its full description and price.
    /// </summary>
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

    // --- helpers ---

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

    /// <summary>
    /// Recursive detailed printing.
    /// If it's a box, prints the box and then dives into children.
    /// If it's a leaf (GiftItemLeaf), uses IGift.Describe() to show decorations.
    /// </summary>
    private void PrintBoxDetails(GiftComponent component, int indentLevel)
    {
        var indent = new string(' ', indentLevel * 2);

        if (component is GiftItemLeaf leaf)
        {
            // Leaf: use the decorated gift's description
            var desc = leaf.Gift.Describe();
            var price = leaf.GetPrice();
            Console.WriteLine($"{indent}- {desc} [{price} MDL]");
        }
        else
        {
            // Composite: show its name and price, then its children
            Console.WriteLine($"{indent}* {component.Name} ({component.GetPrice()} MDL)");

            foreach (var child in component.GetChildren())
            {
                PrintBoxDetails(child, indentLevel + 1);
            }
        }
    }
}
