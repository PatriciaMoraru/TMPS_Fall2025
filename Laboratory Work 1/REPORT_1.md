# **SOLID Principles — Employee Management System**

## **Author:** Patricia Moraru  

---

## **Objectives**
* Understand and apply at least 3 SOLID principles in a simple C# project.  
* Create modular, testable, and extensible code using interfaces, dependency injection, and polymorphism.  
* Demonstrate how applying SOLID improves flexibility and reduces the need for modification when requirements change.  

---

## **Used Principles**
* **SRP — Single Responsibility Principle**  
* **OCP — Open–Closed Principle**  
* **ISP — Interface Segregation Principle**  
*(Bonus: DIP — Dependency Inversion Principle is also naturally used through Unity container and logger abstraction.)*

---

## **Implementation Overview**

The project simulates a small **Employee Management System** with several components representing company roles:
- **AuthManager.cs** – manages user authentication.  
- **Employee.cs** – represents employees and their basic data.  
- **EmployeeFinances.cs** – handles payroll and reward calculations.  
- **EmployeeOperations.cs** – reports working hours and performance.  
- **Logger.cs** – logs actions into a text file via `FileLogger`.  
- **Program.cs** – ties everything together and demonstrates dependency injection via Unity.

---

## **Implemented SOLID Principles**

### **1) SRP – Single Responsibility Principle**

Each class in the system has **one clear reason to change**:
- `AuthManager` → only manages login/logout operations.  
- `EmployeeFinances` → only handles pay, rewards, or stock calculations.  
- `FileLogger` → only logs messages to a text file.  
- `EmployeeOperations` → only reports work hours.

**Example — FileLogger.cs:**
```csharp
public class FileLogger : ILogger
{
    private readonly string _logFilePath;

    public FileLogger()
    {
        _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs.txt");
    }

    public void LogError(string message)
    {
        string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
        File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
    }
}
```
*This class focuses only on logging — not authentication, not payroll, just file output.*

---

### **2) OCP – Open–Closed Principle**

> “Software entities should be open for extension, but closed for modification.”

Originally, pay calculation for different employee types (FTE, PTE, C-Level) required **adding new `if` conditions**, which violated OCP.  
Now, each employee type has its own class implementing `IEmployeeFinances` or `IStockOptions`.

**Example — EmployeeFinances.cs:**
```csharp
public class EmployeeFinancesForFTE : IEmployeeFinances
{
    public double CalculatePay(Employee emp)
    {
        double regular = Math.Min(40, emp.TotalHoursWorked);
        double overtime = Math.Max(0, emp.TotalHoursWorked - 40);
        return (regular * 10) + (overtime * 10 * 1.5);
    }

    public double CalculateRewards(Employee emp)
    {
        return CalculatePay(emp) * 0.05 + 100;
    }
}
```

To add a **new employee type**, you now simply create a new class implementing the proper interface — no changes to existing ones.  
*Classes are open for extension, but closed for modification.*

---

### **3) ISP – Interface Segregation Principle**

> “No client should be forced to depend on methods it does not use.”

Interfaces are split based on functionality:
```csharp
public interface IEmployeeRewards { double CalculateRewards(Employee emp); }
public interface IEmployeeFinances : IEmployeeRewards { double CalculatePay(Employee emp); }
public interface IStockOptions : IEmployeeFinances { double CalculateStockOptions(Employee emp); }
```

- **Regular employees (FTE/PTE)** implement `IEmployeeFinances`.  
- **Contractors** only need rewards → `IEmployeeRewards`.  
- **C-Level employees** extend to `IStockOptions`.

*Each class only implements what it needs.*

---

### **(Bonus) DIP – Dependency Inversion Principle**

> “High-level modules should not depend on low-level modules; both should depend on abstractions.”

The project uses **dependency injection** through the **Unity container**:
```csharp
IUnityContainer container = new UnityContainer();
container.RegisterType<ILogger, FileLogger>();
container.RegisterType<Employee>();

Employee emp = container.Resolve<Employee>();
```

`AuthManager` and `Employee` depend on the **`ILogger` interface**, not a specific implementation:
```csharp
public class AuthManager : IAuthManager
{
    private readonly ILogger _logger;
    public AuthManager(ILogger logger) { _logger = logger; }
}
```

*If we replace `FileLogger` with `DatabaseLogger`, no other class changes.*

---

## **Results / Screenshots**

Example program run:
```
==========================================
       Employee Management Simulation
==========================================
Username: moraru_patricia
Password: maxverstappen
[2025-10-17 20:35:51] User login initiated...
[2025-10-17 20:35:51] [INFO] User 'moraru_patricia' successfully logged in.

Employee full name: Edward Cullen

Enter employee type (FTE / PTE / Contractor / CLevel): FTE
Enter hours worked this week: 37

------------------------------------------
            EMPLOYEE REPORT
------------------------------------------
Name                 | Edward Cullen
Type                 | FTE
Hours Worked         | 37
Pay                  | 370,00 lei
Rewards              | 118,50 lei
------------------------------------------

Performance Summary  [FTE] 37h this period - On track
[2025-10-17 20:36:20] [CALC] Name=Edward Cullen, Type=FTE, Hours=37, Pay=370, Rewards=118,5, Stock=0
[2025-10-17 20:36:20] [INFO] Saved employee. Name=Edward Cullen, Type=FTE, Hours=37
[2025-10-17 20:36:20] [INFO] User successfully logged out.
```

Generated `logs.txt`:
```
[2025-10-17 20:35:51] [INFO] User 'moraru_patricia' successfully logged in.
[2025-10-17 20:36:20] [CALC] Name=Edward Cullen, Type=FTE, Hours=37, Pay=370, Rewards=118,5, Stock=0
[2025-10-17 20:36:20] [INFO] Saved employee. Name=Edward Cullen, Type=FTE, Hours=37
[2025-10-17 20:36:20] [INFO] User successfully logged out.
```

---

## **Conclusions**

* The project demonstrates **three SOLID principles** clearly (SRP, OCP, ISP), with a touch of DIP.  
* Adding new employee types or new loggers requires **no modification** to existing logic — only extension.  
* Each class now has a **clear single purpose**, making the system easy to test and maintain.  
* Dependency injection improves flexibility and decouples modules from specific implementations.  

**Result:** The system follows clean architecture, modularity, and flexibility — key goals of SOLID design.
