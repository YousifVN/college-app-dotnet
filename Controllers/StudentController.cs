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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<Student>> Get()
        {
            return Ok(StudentRepo.LoadStudents);
        }

        [HttpGet("{id:int}")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Student> GetById(int id)
        {
            if (id <= 0) return BadRequest($"The id: {id} is invalid");

            var student = StudentRepo.LoadStudents.FirstOrDefault(e => e.Id == id);
            if (student is null) return NotFound($"The student with the id: {id} not found");
            
            return Ok(student); 
        } 
        
        [HttpGet("{name:alpha}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // returns a list of students with the provided name
        public ActionResult<IEnumerable<Student>> GetByName(string name)
        {
            var students = StudentRepo.LoadStudents.Where(e => e.Name == name);
            return Ok(students);
        } 
        
        // [HttpGet("{email:alpha}")]
        // public IEnumerable<Student> GetByEmail(string email)
        // {
        //     return StudentRepo.LoadStudents.Where(e => e.Name.Contains(email));
        // } 
        
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<bool> DeleteById(int id)
        {
            if (id <= 0) return BadRequest($"The id: {id} is invalid");
                
            var student = StudentRepo.LoadStudents.FirstOrDefault(e => e.Id == id);
            if (student == null) return NotFound($"there is no students with the id: {id}");
            
            StudentRepo.LoadStudents.Remove(student);
            
            return Ok(true);
        } 
        
    }
}
