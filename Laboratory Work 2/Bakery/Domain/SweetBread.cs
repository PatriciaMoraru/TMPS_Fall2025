using Bakery.Services;

namespace Bakery.Domain;

public class SweetBread : Bread
{
    protected override MachineService Service { get; }
    public override string Type => "Sweet";
    public override decimal Price => 5m;

    public SweetBread(MachineService service)
    {
        Service = service;
    }
}