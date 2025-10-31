using Bakery.Services;

namespace Bakery.Domain;

public class ChocolateCake : Cake
{
    protected override MachineService Service { get; }
    public override string Type => "Chocolate";
    public override decimal Price => 15m;

    public ChocolateCake(MachineService service)
    {
        Service = service;
    }
}