using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.domain.school_mgt;
using SchoolManagement.domain.SchoolManagement.dto;
using SchoolManagement.domain.user;
using SchoolManagement.persistent;
using System.Security.Claims;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassRoomController : ControllerBase
    {
        private readonly SchoolManagementDbContext _context;
        public ClassRoomController(SchoolManagementDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Classroom>> CreateClassroom(ClassRoomCreateDto dto)
        {
            var classroomExists = await _context.Classrooms.AnyAsync(c => c.Name == dto.Name);

            if (classroomExists)
            {
                return Conflict(new { message = "Classroom name already exists" });
            }
            var teacherExists = await _context.Users.FindAsync(dto.TeacherId);
            if (teacherExists == null || teacherExists.Role?.ToString() == "Student")
            {
                return NotFound(new { message = "Teacher not found" });
            }
            var classroom = new Classroom
            {
                Name = dto.Name,
                Description = dto.Description,
                TeacherId = dto.TeacherId
            };

            await _context.Classrooms.AddAsync(classroom);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Classroom Created Successfully." });
        }


        [HttpGet]
        public async Task<ActionResult<List<ClassRoomCreateDto>>> GetAllClassrooms()
        {
            var classrooms = await _context.Classrooms
                .Include(c => c.Teacher)
                .Select(c => new ClassRoomCreateDto
                {
                    Name = c.Name,
                    Description = c.Description,
                    TeacherId= c.Teacher.Id
                })
                .ToListAsync();

            return Ok(classrooms);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Classroom>> GetClassroom(int id)
        {
            var classroom = await _context.Classrooms.FindAsync(id);

            if (classroom == null)
            {
                return NotFound($"Classroom with ID {id} not found.");
            }

            return Ok(classroom);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Classroom>> UpdateClassroom(int id, ClassRoomCreateDto dto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var role = identity.FindFirst(ClaimTypes.Role).Value;

            if (role != "Teacher")
            {
                return Unauthorized("You cannot Update this Classroom");
            }

            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom == null)
            {
                return NotFound(new { message = $"Classroom with ID {id} does not exist" });
            }

            classroom.Name = dto.Name;
            classroom.Description = dto.Description;
            classroom.TeacherId = dto.TeacherId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "A concurrency error occurred while updating the teacher." });
            }

            return Ok(classroom);
        }
    }
}
