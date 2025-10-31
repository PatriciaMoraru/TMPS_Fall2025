using System;
using System.Collections.Generic;

namespace Bakery.Services;

public class MachineServicePool
{
    private readonly Queue<MachineService> _available = new();
    private readonly List<MachineService> _inUse = new();

    public MachineServicePool(int size)
    {
        for (int i = 0; i < size; i++)
        {
            _available.Enqueue(new MachineService());
        }
    }

    public MachineService Acquire()
    {
        if (_available.Count == 0)
        {
            Console.WriteLine("No available machines. Please wait...");
            return new MachineService(); // fallback
        }

        var machine = _available.Dequeue();
        _inUse.Add(machine);
        Console.WriteLine("Machine acquired from pool.");
        return machine;
    }

    public void Release(MachineService machine)
    {
        if (_inUse.Remove(machine))
        {
            _available.Enqueue(machine);
            Console.WriteLine("Machine returned to pool.");
        }
    }
}