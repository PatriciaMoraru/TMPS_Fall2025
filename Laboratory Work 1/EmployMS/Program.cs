using Unity;

namespace EmployMS;

class Program
{
    static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("==========================================");
        Console.WriteLine("       Employee Management Simulation");
        Console.WriteLine("==========================================");
        Console.ResetColor();

        IUnityContainer container = new UnityContainer();
        container.RegisterType<ILogger, FileLogger>();
        container.RegisterType<Employee>();
        
        var auth = new AuthManager();
        auth.Login("patricia_moraru", "tmps_fall25");

        if (!auth.IsCurrentUserAuthenticated())
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Login failed. Exiting...");
            Console.ResetColor();
            return;
        }

        var current = auth.GetCurrentLoggedInUser();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\nEnter employee type (FTE / PTE / Contractor / CLevel): ");
        string type = Console.ReadLine()?.Trim() ?? "FTE";

        Console.Write("Enter hours worked this week: ");
        double hours = double.TryParse(Console.ReadLine(), out double h) ? h : 38;
        Console.ResetColor();

        current.EmployeeType = type;
        current.TotalHoursWorked = hours;

        IEmployeeRewards rewardsCalc;
        IEmployeeFinances? financesCalc = null;
        IStockOptions? stockCalc = null;

        switch (type.ToUpper())
        {
            case "FTE":
                financesCalc = new EmployeeFinancesForFTE();
                break;
            case "PTE":
                financesCalc = new EmployeeFinancesForPTE();
                break;
            case "CONTRACTOR":
                rewardsCalc = new EmployeeFinancesForContractor();
                break;
            case "CLEVEL":
                stockCalc = new EmployeeFinancesForCLevel();
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Unknown type. Defaulting to PTE.\n");
                Console.ResetColor();
                financesCalc = new EmployeeFinancesForPTE();
                break;
        }

        double pay = 0, rewards = 0, stock = 0;

        if (financesCalc != null)
        {
            pay = financesCalc.CalculatePay(current);
            rewards = financesCalc.CalculateRewards(current);
        }
        else if (type.Equals("Contractor", StringComparison.OrdinalIgnoreCase))
        {
            rewards = new EmployeeFinancesForContractor().CalculateRewards(current);
        }
        else if (stockCalc != null)
        {
            pay = stockCalc.CalculatePay(current);
            rewards = stockCalc.CalculateRewards(current);
            stock = stockCalc.CalculateStockOptions(current);
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n------------------------------------------");
        Console.WriteLine("            EMPLOYEE REPORT");
        Console.WriteLine("------------------------------------------");
        Console.ResetColor();

        Console.WriteLine($"{ "Type",-20} | {type}");
        Console.WriteLine($"{ "Hours Worked",-20} | {hours}");
        Console.WriteLine($"{ "Pay",-20} | {pay,10:C}");
        Console.WriteLine($"{ "Rewards",-20} | {rewards,10:C}");
        if (stock > 0)
            Console.WriteLine($"{ "Stock Options",-20} | {stock,10:C}");
        Console.WriteLine("------------------------------------------");

        EmployeeOperations ops = new EmployeeOperations();
        string report = ops.ReportHours(current);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\nPerformance Summary → {report}");
        Console.ResetColor();

        current.Save(current);

        auth.Logout();

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\nProcess complete. Press any key to exit...");
        Console.ResetColor();
        Console.ReadKey();
    }
}
