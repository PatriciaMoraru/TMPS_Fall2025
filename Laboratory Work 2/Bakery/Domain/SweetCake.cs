using Bakery.Services;

namespace Bakery.Domain;

public class SweetCake : Cake
{
    protected override MachineService Service { get; }
    public override string Type => "Sweet";
    public override decimal Price => 12m;

    public SweetCake(MachineService service)
    {
        Service = service;
    }
}