namespace EmployMS;

public interface IAuthManager
{
    bool Login(string username, string password);
    void Logout();
    bool IsCurrentUserAuthenticated();
    Employee GetCurrentLoggedInUser();
}

public class AuthManager : IAuthManager
{
    private readonly ILogger _logger;
    private bool _isAuthenticated = false;
    private Employee? _currentUser;
    public AuthManager(ILogger logger)
    {
        _logger = logger;
    }
    
    public AuthManager() : this(new FileLogger()) {}
    
    public bool Login(string username, string password)
    {
        _logger.LogError("User login initiated...");

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            _logger.LogError("[WARN] Login failed: empty username or password.");
            _isAuthenticated = false;
            _currentUser = null;
            return false;
        }

        _isAuthenticated = true;

        _currentUser = new Employee(_logger)
        {
            EmployeeType = "FullTime",
            TotalHoursWorked = 38
        };

        _logger.LogError($"[INFO] User '{username}' successfully logged in.");
        return true;
    }

    public void Logout()
    {
        if (_isAuthenticated)
        {
            _logger.LogError("[INFO] User successfully logged out.");
            _isAuthenticated = false;
            _currentUser = null;
        }
        else
        {
            _logger.LogError("[WARN] No user currently logged in to log out.");
        }
    }

    public bool IsCurrentUserAuthenticated()
    {
        return _isAuthenticated;
    }

    public Employee GetCurrentLoggedInUser()
    {
        if (_isAuthenticated && _currentUser != null)
        {
            return _currentUser;
        }

        _logger.LogError("[WARN] Attempted to get user details but no user is logged in.");
        return new Employee(_logger);
    }
}