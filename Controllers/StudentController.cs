using CollegeApp.Data;
using CollegeApp.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<StudentDTO>>> GetAllAsync()
    {
        _logger.LogInformation("GetStudents method started");
        var students = await _dbContext.Students.Select(s => new StudentDTO()
        {
            Id = s.Id,
            Name = s.Name,
            Address = s.Address,
            Email = s.Email,
            DOB = s.DOB
        }).ToListAsync();
        return Ok(students);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status404NotFound), ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StudentDTO>> GetByIdAsync(int id)
    {
        _logger.LogInformation("GetById method started");
        if (id <= 0)
        {
            _logger.LogWarning("Bad request");
            return BadRequest($"The id: {id} is invalid");
        }

        var student = await _dbContext.Students.Where(n => n.Id == id).FirstOrDefaultAsync();
        if (student is null)
        {
            _logger.LogError("Student is null/ not found");
            return NotFound($"The student with the id: {id} not found");
        }
        
        var studentDto = new StudentDTO
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            Address = student.Address,
            DOB = student.DOB
        };

        return Ok(studentDto);
    }

    [HttpGet("{name:alpha}")]
    [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status404NotFound), ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StudentDTO>> GetByNameAsync(string name)
    {
        if (string.IsNullOrEmpty(name)) return BadRequest();
        
        var student = await _dbContext.Students.Where(n => n.Name == name).FirstOrDefaultAsync();
        
        if (student == null) return NotFound($"The student with name {name} not found");
        
        var studentDto = new StudentDTO
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            Address = student.Address,
            DOB = student.DOB
        };

        return Ok(studentDto);
    }
    
    [HttpPost("Create")]
    [ProducesResponseType(StatusCodes.Status201Created), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StudentDTO>> CreateAsync([FromBody] StudentDTO model)
    {
        if (model is null) return BadRequest("Request body can't be empty");

        var newEntry = new Student
        {
            Name = model.Name,
            Address = model.Address,
            Email = model.Email,
            DOB = model.DOB,
        };

        await _dbContext.Students.AddAsync(newEntry);
        await _dbContext.SaveChangesAsync();
        model.Id = newEntry.Id;
        return Ok(model);
    }
    
    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status204NoContent), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status404NotFound), ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateAsync([FromBody] StudentDTO model)
    {
        if (model is null || model.Id <= 0) return BadRequest();

        var existingEntry = await _dbContext.Students.AsNoTracking().Where(s => s.Id == model.Id).FirstOrDefaultAsync();

        if (existingEntry is null) return NotFound();

        var newEntry = new Student()
        {
            Id = existingEntry.Id,
            Name = model.Name,
            Email = model.Email,
            Address = model.Address,
            DOB = model.DOB
        };
        
        _dbContext.Students.Update(newEntry);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpPatch("Patch/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status404NotFound), ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> PatchAsync(int id, [FromBody] JsonPatchDocument<StudentDTO> model)
    {
        if (model is null || id <= 0) return BadRequest();

        var existingEntry = await _dbContext.Students.Where(e => e.Id == id).FirstOrDefaultAsync();

        if (existingEntry is null) return NotFound();

        var studentDto = new StudentDTO
        {
            Id = existingEntry.Id,
            Name = existingEntry.Name,
            Email = existingEntry.Email,
            Address = existingEntry.Address
        };

        model.ApplyTo(studentDto, ModelState);

        if (!ModelState.IsValid) return BadRequest();

        existingEntry.Name = studentDto.Name;
        existingEntry.Email = studentDto.Email;
        existingEntry.Address = studentDto.Address;
        existingEntry.DOB = studentDto.DOB;

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status404NotFound), ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> DeleteAsync(int id)
    {
        if (id <= 0) return BadRequest($"The id: {id} is invalid");

        var student = await _dbContext.Students.Where(e => e.Id == id).FirstOrDefaultAsync();
        
        if (student == null) return NotFound($"there is no students with the id: {id}");

        _dbContext.Students.Remove(student);
        await _dbContext.SaveChangesAsync();

        return Ok(true);
    }
}