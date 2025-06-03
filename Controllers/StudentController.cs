using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeApp.Models;
using CollegeApp.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet("All")]
        public ActionResult<IEnumerable<Student>> Get()
        {
            return Ok(StudentRepo.LoadStudents);
        }

        [HttpGet("{id:int}")]
        public ActionResult<IEnumerable<Student>> GetById(int id)
        {
            if (id <= 0) return BadRequest($"The id: {id} is invalid");
            return Ok(StudentRepo.LoadStudents.Where(e => e.Id == id)); 
        } 
        
        [HttpGet("{name:alpha}")]
        public ActionResult<IEnumerable<Student>> GetByName(string name)
        {
            return Ok(StudentRepo.LoadStudents.Where(e => e.Name == name));
        } 
        
        // [HttpGet("{email:alpha}")]
        // public IEnumerable<Student> GetByEmail(string email)
        // {
        //     return StudentRepo.LoadStudents.Where(e => e.Name.Contains(email));
        // } 
        
        [HttpDelete("{id:int}")]
        public ActionResult<bool> DeleteById(int id)
        {
            if (id <= 0) return BadRequest($"The id: {id} is invalid");
            var student = StudentRepo.LoadStudents.FirstOrDefault(e => e.Id == id);
            if (student == null)
                return BadRequest($"there is no students with the id: {id}");
            StudentRepo.LoadStudents.Remove(student);
            return Ok(true);
        } 
        
    }
}
