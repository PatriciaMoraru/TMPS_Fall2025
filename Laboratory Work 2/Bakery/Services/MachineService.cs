using Bakery.Models;

namespace Bakery.Services;

public class MachineService
{
    public void OpenDoor() => Console.WriteLine("Opening door...");
    public void WaitForPickup() => Console.WriteLine("Waiting for pickup...");
    public void CloseDoor() => Console.WriteLine("Closing door...");
    public void WaitForMoney(decimal price) => Console.WriteLine($"Waiting for {price} EUR...");
    public void Cook(CroissantSize size) => Console.WriteLine($"Cooking croissant: {size}");
    public void CustomOperation(string command) => Console.WriteLine($"[ROBOT] {command}");
    public void CookBread() => Console.WriteLine("[ROBOT] Baking bread...");
    public void CookCake() => Console.WriteLine("[ROBOT] Baking cake layers...");
}