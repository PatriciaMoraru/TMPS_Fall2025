namespace EmployMS;

// CTO is responsible for Employee class all together
public class Employee
{

    public string EmployeeType { get; set; }
    public double TotalHoursWorked { get; set; }

    public ILogger logger;

    public Employee(ILogger InputLogger)
    {
        logger = InputLogger;
    }

    public void Save(Employee emp)
    {

        try
        {
            //code for saving
            throw new Exception();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);

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

