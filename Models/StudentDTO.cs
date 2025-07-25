using CollegeApp.Validators;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace CollegeApp.Models;

public class StudentDTO
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Address { get; set; }
    
    public DateTime DOB { get; set; }
}