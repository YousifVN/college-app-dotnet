using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CollegeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CollegeApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class LoginController : Controller
{
    private readonly IConfiguration _configuration;

    public LoginController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [HttpPost]
    public ActionResult Login(LoginDTO model)
    {
        if (!ModelState.IsValid) return BadRequest("Please provide username and password");

        LoginResponseDTO response = new LoginResponseDTO() { Username = model.Username };
        
        if (model.Username == "admin" && model.Password == "1234")
        {
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecret"));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.Role, "Admin")

                }),
                Expires = DateTime.Now.AddHours(4),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            response.token = tokenHandler.WriteToken(token);
        }
        else
        {
            return Ok("Invalid username and password");
        }

        return Ok(response);
    }
}