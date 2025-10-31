using Bakery.Domain;
using Bakery.Models;
using Bakery.Services;

namespace Bakery.Factory;

public class ChocolateBakeryFactory : IBakeryFactory
{
    public Croissant CreateCroissant(MachineService service, CroissantCustomization customization)
    {
        return new ChocolateCroissant(service, 10, customization);
    }

    public Bread CreateBread(MachineService service)
    {
        return new ChocolateBread(service);
    }

    public Cake CreateCake(MachineService service)
    {
        return new ChocolateCake(service);
    }
}