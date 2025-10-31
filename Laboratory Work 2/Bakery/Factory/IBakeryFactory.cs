using Bakery.Domain;
using Bakery.Models;
using Bakery.Services;

namespace Bakery.Factory;

public interface IBakeryFactory
{
    Croissant CreateCroissant(MachineService service, CroissantCustomization customization);
    Bread CreateBread(MachineService service);
    Cake CreateCake(MachineService service);
}