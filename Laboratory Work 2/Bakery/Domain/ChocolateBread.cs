using Bakery.Services;

namespace Bakery.Domain;

public class ChocolateBread : Bread
{
    protected override MachineService Service { get; }
    public override string Type => "Chocolate";
    public override decimal Price => 6m;

    public ChocolateBread(MachineService service)
    {
        Service = service;
    }
}