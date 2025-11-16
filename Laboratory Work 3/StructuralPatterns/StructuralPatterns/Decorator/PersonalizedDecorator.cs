namespace StructuralPatterns.Decorator;

/// <summary>
/// Adds a personalized card to the gift.
/// </summary>
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