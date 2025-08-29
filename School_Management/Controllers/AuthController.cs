using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolManagement.domain.SchoolManagement.dto;
using SchoolManagement.domain.user;
using SchoolManagement.persistent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SchoolManagement.domain.SchoolManagementMappers;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SchoolManagementDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(SchoolManagementDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register/")]
        public async Task<ActionResult> Register([FromBody] UserCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingUser = await _context.Users.AnyAsync(s => s.Email == dto.Email);
            if (existingUser)
            {
                return BadRequest(new { message = "This Email already exists in the system." });
            }

            var user = dto.ToEntity();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User created successfully. ", UserId = user.Id });
        }


        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest(new { message = "Email and Password are required." });
            }

            var user = await _context.Users.FirstOrDefaultAsync(s => s.Email == loginRequest.Email);
            if (user != null && VerifyPassword(loginRequest.Password, user.PasswordHash!))
            {
                string Role = user.Role?.ToString();
                var token = GenerateJwtToken(user.Email, user.Id.ToString(), Role);
                return Ok(new LoginResponseDto
                {
                    Token = token,
                    Email = user.Email,
                    Id = user.Id,
                    Role = Role
                });
            }
            return Unauthorized(new { message = "Invalid Email or Password." });
        }

        [HttpGet("me")]
        [Authorize]
        public ActionResult<CurrentUserDto> GetCurrentUser(string role)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (role != identity.FindFirst(ClaimTypes.Role).Value)
            {
                return Unauthorized();
            }
            var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var email = identity.FindFirst(ClaimTypes.Email).Value;
            role = identity.FindFirst(ClaimTypes.Role).Value;


            var currentUser = new CurrentUserDto
            {
                Id = int.Parse(userId),
                Email = email,
                Role = role
            };
            return Ok(currentUser);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash))
            {
                return false;
            }
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, storedHash);
            return isPasswordValid;
        }

        private string GenerateJwtToken(string email, string userId, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
           };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Issuer"],
                audience: _configuration["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
