using CollegeApp.Models;

namespace CollegeApp.Repositories;

public static class StudentRepo
{
    public static List<Student> LoadStudents { get; set; } = new List<Student>()
    {
        new Student { Id = 1, Name = "yousif", Email = "yousif@gmail.com", Address = "Baghdad" },
        new Student { Id = 2, Name = "ali", Email = "ali@gmail.com", Address = "Erbil" },
        new Student { Id = 3, Name = "hassan", Email = "hassan@gmail.com", Address = "Basra" },
        new Student { Id = 3, Name = "ali", Email = "ali2@gmail.com", Address = "Samawah" }
    }; 
}