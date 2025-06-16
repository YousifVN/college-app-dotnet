using CollegeApp.Models;

namespace CollegeApp.Repositories;

public static class StudentRepo
{
    public static List<Student> LoadStudents { get; set; } = new()
    {
        new() { Id = 1, Name = "yousif", Email = "yousif@gmail.com", Address = "Baghdad" },
        new() { Id = 2, Name = "ali", Email = "ali@gmail.com", Address = "Erbil" },
        new() { Id = 3, Name = "hassan", Email = "hassan@gmail.com", Address = "Basra" },
        new() { Id = 4, Name = "ali", Email = "ali2@gmail.com", Address = "Samawah" }
    };
}