using System.ComponentModel.DataAnnotations;
using CollegeApp.Validators;

namespace CollegeApp.Models;

public class Student
{
    public int Id { get; set; }
    
    [Required, StringLength(30)]
    public string Name { get; set; }
    
    [EmailAddress, Required]
    public string Email { get; set; }
    
    [Required]
    public string Address { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }
    
    [DateCheck]
    public DateTime AdmissionDate { get; set; }
}