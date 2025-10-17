using System;
using System.IO;

namespace EmployMS;

public interface ILogger
{
    void LogError(string message);
}

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

        Console.WriteLine(logEntry);

        try
        {
            File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Logger Error] Could not write to log file: {ex.Message}");
            Console.ResetColor();
        }
    }
}

public class DatabaseLogger : ILogger
{
    public void LogError(string message)
    {
        Console.WriteLine($"[Database Log] {message}");
    }
}