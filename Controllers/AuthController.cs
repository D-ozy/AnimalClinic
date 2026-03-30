using AnimalClinicLogic.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AnimalClinicLogic;
using AnimalClinicLogic.Services;
using AnimalClinicLogic.Models;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AnimalClinic.Controllers
{
    [ApiController]
    [Route("auth")]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly DB db;
        private readonly AuthService service;

        public AuthController(DB db, AuthService service)
        {
            this.db = db;
            this.service = service;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration(UserDto userData)
        {
            if (userData == null)
            {
                Response.StatusCode = 400;
                return new ObjectResult(new { message = "Incorrect data" });
            }

            await service.CreateUser(userData);
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto userData)
        {
            if(string.IsNullOrEmpty(userData.NameOrEmail) || string.IsNullOrEmpty(userData.Password))
            {
                Response.StatusCode = 400;
                return new ObjectResult(new { message = "Incorrect data" });
            }

            User user =  await service.GetUser(userData);

            if (user == null)
            {
                Response.StatusCode = 400;
                return new ObjectResult(new { message = "Incorrect name or password" });
            }

            Claim[] claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("f8Dk2!sL9@qPzX7#vB4mN6cR1tYwE3uH")
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(24),
                    signingCredentials: creds
                );

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);

            Response.Cookies.Append("jwt", jwt, new CookieOptions { HttpOnly = true, Secure = true });

            return Ok(new { token = jwt });
        }
    }
}
