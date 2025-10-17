namespace EmployMS;

// CFO is repsonsible for the EmployeeFinances class (calculating the salary)
public interface IEmployeeRewards
{
    double CalculateRewards(Employee emp);
}

public interface IEmployeeFinances : IEmployeeRewards
{
    double CalculatePay(Employee emp);
}

public interface IStockOptions : IEmployeeFinances
{
    double CalculateStockOptions(Employee emp);
}

public class EmployeeFinancesForFTE : IEmployeeFinances
{
    // keep your original rate but add overtime logic
    private const double HOURLY_RATE = 10.0;
    private const double OVERTIME_MULTIPLIER = 1.5;

    public double CalculatePay(Employee emp)
    {
        if (emp == null) return 0;
        var hours = SanitizeHours(emp.TotalHoursWorked);

        var regular = Math.Min(40.0, hours);
        var overtime = Math.Max(0.0, hours - 40.0);

        var pay = (regular * HOURLY_RATE) + (overtime * HOURLY_RATE * OVERTIME_MULTIPLIER);
        return Round2(pay);
    }

    public double CalculateRewards(Employee emp)
    {
        if (emp == null) return 0;
        // small % of pay + a modest fixed bonus (keeps your “200” spirit but makes it dynamic)
        var pay = CalculatePay(emp);
        var rewards = (pay * 0.05) + 100.0;
        return Round2(rewards);
    }

    private static double SanitizeHours(double h) =>
        (!double.IsFinite(h) || h < 0) ? 0 : h;

    private static double Round2(double v) => Math.Round(v, 2, MidpointRounding.AwayFromZero);
}

public class EmployeeFinancesForPTE : IEmployeeFinances
{
    private const double HOURLY_RATE = 5.0;

    public double CalculatePay(Employee emp)
    {
        if (emp == null) return 0;
        var hours = SanitizeHours(emp.TotalHoursWorked);
        var pay = hours * HOURLY_RATE; // no overtime for PTE
        return Round2(pay);
    }

    public double CalculateRewards(Employee emp)
    {
        if (emp == null) return 0;
        // smaller % and a gentle floor so it isn't zero for tiny hours
        var pay = CalculatePay(emp);
        var rewards = Math.Max(50.0, pay * 0.03); // floor at 50
        return Round2(rewards);
    }

    private static double SanitizeHours(double h) =>
        (!double.IsFinite(h) || h < 0) ? 0 : h;

    private static double Round2(double v) => Math.Round(v, 2, MidpointRounding.AwayFromZero);
}

public class EmployeeFinancesForContractor : IEmployeeRewards
{
    public double CalculateRewards(Employee emp)
    {
        if (emp == null) return 0;
        var hours = SanitizeHours(emp.TotalHoursWorked);
        // contractors have no salary here; give a milestone-style bonus that scales gently with hours
        var baseBonus = 100.0;
        var performance = Math.Min(200.0, hours * 1.0); // +1 per hour, capped at +200
        return Round2(baseBonus + performance);
    }

    private static double SanitizeHours(double h) =>
        (!double.IsFinite(h) || h < 0) ? 0 : h;

    private static double Round2(double v) => Math.Round(v, 2, MidpointRounding.AwayFromZero);
}

public class EmployeeFinancesForCLevel : IStockOptions
{
    // executives: fixed-period salary, % bonus, simple stock grant
    private const double PERIOD_SALARY = 4000.0;

    public double CalculatePay(Employee emp)
    {
        // fixed salary per period
        return PERIOD_SALARY;
    }

    public double CalculateRewards(Employee emp)
    {
        // 10% cash bonus of salary
        return Round2(PERIOD_SALARY * 0.10);
    }

    public double CalculateStockOptions(Employee emp)
    {
        var hours = SanitizeHours(emp?.TotalHoursWorked ?? 0);
        // base grant + a tiny kicker based on “engagement” hours
        var stock = 1000.0 + Math.Floor(hours / 10.0) * 100.0;
        return Round2(stock);
    }

    private static double SanitizeHours(double h) =>
        (!double.IsFinite(h) || h < 0) ? 0 : h;

    private static double Round2(double v) => Math.Round(v, 2, MidpointRounding.AwayFromZero);
}
