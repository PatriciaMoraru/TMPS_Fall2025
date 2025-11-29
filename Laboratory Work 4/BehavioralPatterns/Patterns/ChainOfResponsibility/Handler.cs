using KitchenOrderApp.Domain;

namespace KitchenOrderApp.Patterns.ChainOfResponsibility;

public abstract class Handler
{
    protected Handler? successor;

    public void SetSuccessor(Handler successor)
    {
        this.successor = successor;
    }

    public abstract void Handle(Order order);
}

