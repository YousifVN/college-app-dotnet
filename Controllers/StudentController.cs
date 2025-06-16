using CollegeApp.Data;
using CollegeApp.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly ILogger<StudentController> _logger;
    private readonly CollegeDbContext _dbContext; 

    public StudentController(ILogger<StudentController> logger, CollegeDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpGet("All")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<Student>> Get()
    {
        _logger.LogInformation("GetStudents method started");
        return Ok(_dbContext.Students);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Student> GetById(int id)
    {
        _logger.LogInformation("GetById method started");
        if (id <= 0)
        {
            _logger.LogWarning("Bad request");
            return BadRequest($"The id: {id} is invalid");
        }

        var student = _dbContext.Students.FirstOrDefault(e => e.Id == id);
        if (student is null)
        {
            _logger.LogError("Student is null/ not found");
            return NotFound($"The student with the id: {id} not found");
        }

        return Ok(student);
    }

    [HttpGet("{name:alpha}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    // returns a list of students with the provided name
    public ActionResult<IEnumerable<Student>> GetByName(string name)
    {
        var students = _dbContext.Students.Where(e => e.Name == name);
        return Ok(students);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<bool> DeleteById(int id)
    {
        if (id <= 0) return BadRequest($"The id: {id} is invalid");

        var student = _dbContext.Students.FirstOrDefault(e => e.Id == id);
        if (student == null) return NotFound($"there is no students with the id: {id}");

        _dbContext.Students.Remove(student);
        _dbContext.SaveChanges();

        return Ok(true);
    }

    [HttpPost("Create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Student> CreateStudent([FromBody] Student model)
    {
        if (model is null) return BadRequest("Request body can't be empty");

        var newEntry = new Student
        {
            Name = model.Name,
            Address = model.Address,
            Email = model.Email,
            DOB = model.DOB,
        };

        _dbContext.Students.Add(newEntry);
        _dbContext.SaveChanges();
        model.Id = newEntry.Id;
        return Ok(model);
    }

    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult UpdateStudent([FromBody] Student model)
    {
        if (model is null || model.Id <= 0) return BadRequest();

        var existingStudent = _dbContext.Students.FirstOrDefault(e => e.Id == model.Id);

        if (existingStudent is null) return NotFound();

        existingStudent.Name = model.Name;
        existingStudent.Email = model.Email;
        existingStudent.Address = model.Address;
        existingStudent.DOB = model.DOB;

        _dbContext.SaveChanges();

        return NoContent();
    }

    [HttpPatch("Patch/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult PatchStudent(int id, [FromBody] JsonPatchDocument<Student> model)
    {
        if (model is null || id <= 0) return BadRequest();

        var existingStudent = _dbContext.Students.FirstOrDefault(e => e.Id == id);

        if (existingStudent is null) return NotFound();

        var student = new Student
        {
            Id = existingStudent.Id,
            Name = existingStudent.Name,
            Email = existingStudent.Email,
            Address = existingStudent.Address,
            DOB = existingStudent.DOB
            
        };

        model.ApplyTo(student, ModelState);

        if (!ModelState.IsValid) return BadRequest();

        existingStudent.Name = student.Name;
        existingStudent.Email = student.Email;
        existingStudent.Address = student.Address;
        existingStudent.DOB = student.DOB;

        _dbContext.SaveChanges();

        return NoContent();
    }
}