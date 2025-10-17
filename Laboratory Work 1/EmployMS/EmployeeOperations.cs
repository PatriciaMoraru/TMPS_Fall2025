namespace EmployMS;

// COO is responsible for the EmployeeOperations class (i.e checking the hours worked by employees)
public class EmployeeOperations
{
    public string ReportHours(Employee emp)
    {
        if (emp == null)
            return "No employee provided.";

        double hours = emp.TotalHoursWorked;

        if (double.IsNaN(hours) || double.IsInfinity(hours) || hours < 0)
            return $"Invalid hours value: {hours}.";

        string status;
        if (hours < 20)        status = "Under target";
        else if (hours <= 40)  status = "On track";
        else                   status = "Overtime";

        string type = string.IsNullOrWhiteSpace(emp.EmployeeType) ? "Unknown" : emp.EmployeeType;

        return $"[{type}] {hours:0.##}h this period — {status}";
    }

}
