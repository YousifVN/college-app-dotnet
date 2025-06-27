using System.Net;
using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICollegeRepository<Role> _roleRepository;
        private APIResponse _apiResponse;
        
        public RoleController(IMapper mapper, ICollegeRepository<Role> roleRepository)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
            _apiResponse = new APIResponse();
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateRoleAsync(RoleDTO dto)
        {
            try
            {
                if (dto == null) return BadRequest();

                Role role = _mapper.Map<Role>(dto);
                role.IsDeleted = false;
                role.CreationDate = DateTime.Now;
                role.ModificationDate = DateTime.Now;

                var result = await _roleRepository.CreateAsync(role);

                dto.Id = result.Id;

                _apiResponse.Data = dto;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                // return CreatedAtRoute("GetRoleById", new { id = dto.Id }, _apiResponse);

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);

                return _apiResponse;
            }
        }

        [HttpGet]
        [Route("All", Name = "GetAllRoles")]
        public async Task<ActionResult<APIResponse>> GetRolesAsync()
        {
            try
            {
                var roles = await _roleRepository.GetAllAsync();

                _apiResponse.Data = _mapper.Map<List<RoleDTO>>(roles);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
            
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);

                return _apiResponse;
            }
        }
        
        [HttpGet]
        [Route("{id:int}", Name = "GetRoleById")]
        public async Task<ActionResult<APIResponse>> GetRolesAsync(int id)
        {
            try
            {
                if (id <= 0) return BadRequest();
                
                var role = await _roleRepository.GetAsync(role => role.Id == id);

                if (role == null) return NotFound($"There is no Role with the id: {id}");

                _apiResponse.Data = _mapper.Map<RoleDTO>(role);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
            
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);

                return _apiResponse;
            }
        }
        
        [HttpGet]
        [Route("{name:alpha}", Name = "GetRoleByName")]
        public async Task<ActionResult<APIResponse>> GetRolesAsync(string name)
        {
            try
            {
                if (String.IsNullOrEmpty(name)) return BadRequest();
                
                var role = await _roleRepository.GetAsync(role => role.RoleName == name);

                if (role == null) return NotFound($"There is no Role with the name: {name}");

                _apiResponse.Data = _mapper.Map<RoleDTO>(role);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
            
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);

                return _apiResponse;
            }
        }
    }
}
