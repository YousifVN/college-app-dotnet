namespace CollegeApp.MyLogger;

public class LogToMemory : IMyLogger
{
    public void Log(string message)
    {
        Console.WriteLine($"Memory Logging: {message}");
    }
}