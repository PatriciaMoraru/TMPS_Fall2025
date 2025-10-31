using Bakery.Domain;
using Bakery.Models;
using Bakery.Services;

namespace Bakery.Factory;

public class SweetBakeryFactory : IBakeryFactory
{
    public Croissant CreateCroissant(MachineService service, CroissantCustomization customization)
    {
        return new SweetCroissant(service, 5, customization);
    }

    public Bread CreateBread(MachineService service)
    {
        return new SweetBread(service);
    }

    public Cake CreateCake(MachineService service)
    {
        return new SweetCake(service);
    }
}