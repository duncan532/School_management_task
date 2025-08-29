using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.domain.school_mgt;
using SchoolManagement.domain.SchoolManagement.dto;
using SchoolManagement.persistent;
using System.Security.Claims;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly SchoolManagementDbContext _context;
        public CourseController(SchoolManagementDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> CreateCourse(CourseCreateDto dto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var role = identity.FindFirst(ClaimTypes.Role).Value;

            if (role != "Teacher")
            {
                return Unauthorized("You cannot create this course");
            }
            var courseExists = await _context.Courses.AnyAsync(c => c.Name == dto.Name);
            if (courseExists)
            {
                return Conflict(new { message = "Course name already exists" });
            }
            var course = new Course
            {
                Name = dto.Name,
                Description = dto.Description,
            };
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Course Created Successfully." });
        }

        [HttpGet]
        public async Task<ActionResult<List<CourseReadDto>>> GetAllCourses()
        {
            var courses = await _context.Courses
                .Include(c => c.Teacher)
                .Select(c => new CourseReadDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Teacher = c.Teacher == null ? null : new UserReadDto
                    {
                        Id = c.Teacher.Id,
                        FirstName = c.Teacher.FirstName,
                        LastName = c.Teacher.LastName
                    }
                })
                .ToListAsync();

            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseReadDto>> GetCourseById(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Teacher)
                .Where(c => c.Id == id)
                .Select(c => new CourseReadDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Teacher = c.Teacher == null ? null : new UserReadDto
                    {
                        Id = c.Teacher.Id,
                        FirstName = c.Teacher.FirstName,
                        LastName = c.Teacher.LastName
                    }
                })
                .FirstOrDefaultAsync();
            if (course == null)
            {
                return NotFound(new { message = "Course not found" });
            }
            return Ok(course);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCourse(int id, CourseCreateDto dto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var role = identity.FindFirst(ClaimTypes.Role).Value;

            if (role != "Teacher")
            {
                return Unauthorized("You cannot Update this Classroom");
            }
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound(new { message = "Course not found" });
            }
            var courseExists = await _context.Courses.AnyAsync(c => c.Name == dto.Name && c.Id != id);
            if (courseExists)
            {
                return Conflict(new { message = "Course name already exists" });
            }
            course.Name = dto.Name;
            course.Description = dto.Description;
            course.TeacherId = dto.TeacherId;
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Course Updated Successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var role = identity.FindFirst(ClaimTypes.Role).Value;
            if (role != "Teacher")
            {
                return Unauthorized("You cannot Delete this Course");
            }
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound(new { message = "Course not found" });
            }
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Course Deleted Successfully." });
        }
    }
}
