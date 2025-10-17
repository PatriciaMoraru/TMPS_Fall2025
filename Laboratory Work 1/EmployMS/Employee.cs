namespace EmployMS;

// CTO is responsible for Employee class all together
public class Employee
{

    public string EmployeeType { get; set; }
    public double TotalHoursWorked { get; set; }
    public string FullName { get; set; } = "Unknown";
    

    private readonly ILogger _logger;

    public Employee(ILogger InputLogger)
    {
        _logger = InputLogger;
    }

    public void Save(Employee emp)
    {
        try
        {
            if (emp == null)
            {
                _logger.LogError("[ERROR] Save failed: employee is null.");
                return;
            }

            if (emp.TotalHoursWorked < 0)
            {
                _logger.LogError($"[WARN] Save blocked: negative hours ({emp.TotalHoursWorked}).");
                return;
            }

            _logger.LogError($"[INFO] Saved employee. Name={emp.FullName}, Type={emp.EmployeeType}, Hours={emp.TotalHoursWorked:0.##}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"[ERROR] Save encountered an exception: {ex.Message}");
        }
    }
       
}

public enum empType
{
    FullTime,
    PartTime,
    Contractor,
    CLevel
}

