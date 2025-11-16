using StructuralPatterns.Decorator;

namespace StructuralPatterns.Proxy;

/// <summary>
/// Proxy for a gift that is expensive to create or fetch.
/// It delays creating the real gift until price/description is actually needed.
/// </summary>
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

    /// <summary>
    /// The name we show in the catalog before the real gift is loaded.
    /// </summary>
    public string Name => _displayName + " (proxy)";

    /// <summary>
    /// Lazily-created real gift. Created only once when needed.
    /// </summary>
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
