using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
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
    private readonly IMapper _mapper;
    private readonly ICollegeRepository<Student> _studentRepository;

    public StudentController(ILogger<StudentController> logger, IMapper mapper, ICollegeRepository<Student> studentRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _studentRepository = studentRepository;
    }

    [HttpGet("All")]
    [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<StudentDTO>>> GetAllAsync()
    {
        _logger.LogInformation("GetStudents method started");

        var students = await _studentRepository.GetAllAsync();

        var studentDtoData = _mapper.Map<List<StudentDTO>>(students);
        
        return Ok(studentDtoData);
    }

    [HttpGet]
    [Route("{id:int}", Name = "GetStudentById")]
    [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status404NotFound), ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StudentDTO>> GetByIdAsync(int id)
    {
        _logger.LogInformation("GetById method started");
        if (id <= 0)
        {
            _logger.LogWarning("Bad request");
            return BadRequest($"The id: {id} is invalid");
        }

        var student = await _studentRepository.GetByIdAsync(student => student.Id == id);
        
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

        var student = await _studentRepository.GetByNameAsync(student => student.Name.ToLower().Contains(name.ToLower()));
        
        if (student == null) return NotFound($"The student with name {name} not found");
        
        var studentDto = _mapper.Map<StudentDTO>(student);

        return Ok(studentDto);
    }
    
    [HttpPost("Create")]
    [ProducesResponseType(StatusCodes.Status201Created), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StudentDTO>> CreateAsync([FromBody] StudentDTO dto)
    {
        if (dto is null) return BadRequest("Request body can't be empty");

        var student = _mapper.Map<Student>(dto);

        var studentAfterCreation = await _studentRepository.CreateAsync(student);

        dto.Id = studentAfterCreation.Id;
        // Status - 201
        // https://localhost:7185/api/Student/3
        // New student details
        return CreatedAtRoute("GetStudentById", new { id = dto.Id }, dto);
    }
    
    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status204NoContent), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status404NotFound), ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateAsync([FromBody] StudentDTO dto)
    {
        if (dto is null || dto.Id <= 0) return BadRequest();

        var existingEntry = await _studentRepository.GetByIdAsync(student => student.Id == dto.Id, true);

        if (existingEntry is null) return NotFound();

        var newEntry = _mapper.Map<Student>(dto);

        await _studentRepository.UpdateAsync(newEntry);

        return NoContent();
    }
    
    [HttpPatch("Patch/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status404NotFound), ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> PatchAsync(int id, [FromBody] JsonPatchDocument<StudentDTO> dto)
    {
        if (dto is null || id <= 0) return BadRequest();

        var existingEntry = await _studentRepository.GetByIdAsync(student => student.Id == id, true);

        if (existingEntry is null) return NotFound();

        var studentDto = _mapper.Map<StudentDTO>(existingEntry);

        dto.ApplyTo(studentDto, ModelState);

        if (!ModelState.IsValid) return BadRequest();

        existingEntry = _mapper.Map<Student>(studentDto);

        await _studentRepository.UpdateAsync(existingEntry);
        
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status404NotFound), ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> DeleteAsync(int id)
    {
        if (id <= 0) return BadRequest($"The id: {id} is invalid");

        var student = await _studentRepository.GetByIdAsync(student => student.Id == id);
        
        if (student == null) return NotFound($"there is no students with the id: {id}");

        await _studentRepository.DeleteAsync(student);

        return Ok(true);
    }
}