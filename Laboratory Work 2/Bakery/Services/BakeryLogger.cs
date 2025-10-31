using System;

namespace Bakery.Services;
public sealed class BakeryLogger
{
    private static BakeryLogger? _instance;
    private static readonly object _lock = new();

    private BakeryLogger() { }

    public static BakeryLogger Instance
    {
        get
        {
            lock (_lock)
            {
                return _instance ??= new BakeryLogger();
            }
        }
    }

    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {message}");
    }
}