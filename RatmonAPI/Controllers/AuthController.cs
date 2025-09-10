using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RatmonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config) => _config = config;

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            //2 users hardcoded for demo
            if (request.Username == "admin" && request.Password == "password")
            {
                var token = GenerateJwtToken(request.Username, "Admin");
                return Ok(new { token });
            }
            if (request.Username == "user" && request.Password == "password")
            {
                var token = GenerateJwtToken(request.Username, "User");
                return Ok(new { token });
            }

            return Unauthorized();
        }

        private string GenerateJwtToken(string username, string role)
        {
            var key = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"] ?? "RatmonAPI";

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var keyBytes = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(keyBytes, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public record LoginRequest(string Username, string Password);
}
