namespace CollegeApp.MyLogger;

public class LogToDb : IMyLogger
{
    public void Log(string message)
    {
        Console.WriteLine($"DB Logging: {message}");
    }
}