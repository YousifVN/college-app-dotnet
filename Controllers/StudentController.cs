using AutoMapper;
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
    private readonly IMapper _mapper;

    public StudentController(ILogger<StudentController> logger, CollegeDbContext dbContext, IMapper mapper)
    {
        _logger = logger;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet("All")]
    [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<StudentDTO>>> GetAllAsync()
    {
        _logger.LogInformation("GetStudents method started");
        
        var students = await _dbContext.Students.ToListAsync();

        var studentDtoData = _mapper.Map<List<StudentDTO>>(students);
        
        return Ok(studentDtoData);
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
        
        var studentDto = _mapper.Map<StudentDTO>(student);

        return Ok(studentDto);
    }

    [HttpGet("{name:alpha}")]
    [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status404NotFound), ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StudentDTO>> GetByNameAsync(string name)
    {
        if (string.IsNullOrEmpty(name)) return BadRequest();
        
        var student = await _dbContext.Students.Where(n => n.Name == name).FirstOrDefaultAsync();
        
        if (student == null) return NotFound($"The student with name {name} not found");
        
        var studentDto = _mapper.Map<StudentDTO>(student);

        return Ok(studentDto);
    }
    
    [HttpPost("Create")]
    [ProducesResponseType(StatusCodes.Status201Created), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StudentDTO>> CreateAsync([FromBody] StudentDTO dto)
    {
        if (dto is null) return BadRequest("Request body can't be empty");

        var newEntry = _mapper.Map<Student>(dto);

        await _dbContext.Students.AddAsync(newEntry);
        await _dbContext.SaveChangesAsync();
        
        dto.Id = newEntry.Id;
        
        return Ok(dto);
    }
    
    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status204NoContent), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status404NotFound), ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateAsync([FromBody] StudentDTO dto)
    {
        if (dto is null || dto.Id <= 0) return BadRequest();

        var existingEntry = await _dbContext.Students.AsNoTracking().Where(s => s.Id == dto.Id).FirstOrDefaultAsync();

        if (existingEntry is null) return NotFound();

        var newEntry = _mapper.Map<Student>(dto);
        
        _dbContext.Students.Update(newEntry);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpPatch("Patch/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status404NotFound), ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> PatchAsync(int id, [FromBody] JsonPatchDocument<StudentDTO> model)
    {
        if (model is null || id <= 0) return BadRequest();

        var existingEntry = await _dbContext.Students.AsNoTracking().Where(e => e.Id == id).FirstOrDefaultAsync();

        if (existingEntry is null) return NotFound();

        var studentDto = _mapper.Map<StudentDTO>(existingEntry);

        model.ApplyTo(studentDto, ModelState);

        if (!ModelState.IsValid) return BadRequest();

        existingEntry = _mapper.Map<Student>(studentDto);

        _dbContext.Students.Update(existingEntry);
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