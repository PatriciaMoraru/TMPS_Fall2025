using Bakery.Models;
using Bakery.Services;

namespace Bakery.Domain;
    
public abstract class Croissant
{
    public abstract decimal Price { get; }
    public CroissantSize Size
    {
        get => Customization.Size;
        set => Customization.Size = value;
    }
    public abstract CroissantType Type { get; }

    public void Make()
    {
        BakeryLogger.Instance.Log($"Starting to make {Type} croissant...");

        Service.OpenDoor();
        Service.WaitForPickup();
        Service.CloseDoor();
        CustomOperation();
        Service.WaitForMoney(Price);
        Service.Cook(Customization.Size);

        BakeryLogger.Instance.Log($"{Type} croissant finished.\n");
    }

    protected abstract MachineService Service { get; }
    protected abstract CroissantCustomization Customization { get; set; }
    protected abstract void CustomOperation();
}

