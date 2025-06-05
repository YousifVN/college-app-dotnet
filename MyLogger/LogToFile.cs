namespace CollegeApp.MyLogger;

public class LogToFile : IMyLogger
{
    public void Log(string message)
    {
        Console.WriteLine($"File Logging: {message}");
    }
}