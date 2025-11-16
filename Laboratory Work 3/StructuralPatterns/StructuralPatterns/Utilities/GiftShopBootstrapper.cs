using StructuralPatterns.Composite;
using StructuralPatterns.Decorator;
using StructuralPatterns.Proxy;

namespace StructuralPatterns.Utilities;

/// <summary>
/// Helper class that builds a demo catalog with some gift boxes and items.
/// </summary>
public static class GiftShopBootstrapper
{
    public static GiftBoxComposite CreateDemoCatalog()
    {
        // Root: entire catalog
        var catalog = new GiftBoxComposite("Gift Catalog");

        // High-level boxes
        var birthdayBox = new GiftBoxComposite("Birthday Box");
        var christmasBox = new GiftBoxComposite("Christmas Box");

        catalog.Add(birthdayBox);
        catalog.Add(christmasBox);

        // --- Birthday Box contents ---

        // Gift 1: Chocolate box with wrapping and ribbon
        IGift choco = new BasicGift("Premium Chocolate Box", 150m);
        choco = new WrappingPaperDecorator(choco, 20m);
        choco = new RibbonDecorator(choco, 10m);
        var chocoLeaf = new GiftItemLeaf(choco);

        // Gift 2: Mug with card and small discount
        IGift mug = new BasicGift("Cute Birthday Mug", 120m);
        mug = new PersonalizedCardDecorator(mug, "Happy Birthday!");
        mug = new DiscountDecorator(mug, 0.95m); // 5% off
        var mugLeaf = new GiftItemLeaf(mug);

        birthdayBox.Add(chocoLeaf);
        birthdayBox.Add(mugLeaf);

        // --- Christmas Box contents ---

        // Gift 3: Coffee set with full decoration
        IGift coffeeSet = new BasicGift("Christmas Coffee Set", 200m);
        coffeeSet = new WrappingPaperDecorator(coffeeSet, 25m);
        coffeeSet = new RibbonDecorator(coffeeSet, 10m);
        coffeeSet = new PersonalizedCardDecorator(coffeeSet, "Merry Christmas!", 15m);
        var coffeeLeaf = new GiftItemLeaf(coffeeSet);

        // Gift 4: Socks with big discount (lol)
        IGift socks = new BasicGift("Funny Christmas Socks", 80m);
        socks = new DiscountDecorator(socks, 0.8m); // 20% off
        var socksLeaf = new GiftItemLeaf(socks);

        christmasBox.Add(coffeeLeaf);
        christmasBox.Add(socksLeaf);

        // Gift 5: Mystery Box from an external partner, created lazily via Proxy
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


        return catalog;
    }
}
