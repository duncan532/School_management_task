using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.domain.SchoolManagement.dto;
using SchoolManagement.domain.user;
using SchoolManagement.persistent;
using System.Security.Claims;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly SchoolManagementDbContext _context;
        public UserController(SchoolManagementDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            return await _context.Users.Include(s => s.Enrollments).ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetStudentById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User is not Found");
            }
            return user;
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUser(int id, UserCreateDto dto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var role = identity.FindFirst(ClaimTypes.Role).Value;

            if (role != "Teacher" && userId != id.ToString())
            {
                return Unauthorized("You cannot Update the Student");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} does not exist" });
            }
            var emailExists = await _context.Users.AnyAsync(s => s.Email == dto.Email && s.Id != id);
            if (emailExists)
            {
                return BadRequest(new { message = "Email is already in use by another student" });
            }

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.Age = dto.Age;
            user.Phone = dto.Phone;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password); ;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "A concurrency error occurred while updating the student." });
            }
            return Ok(user);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"user with ID {id} does not exist" });
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();

        }

    }
}
