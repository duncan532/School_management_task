using SchoolManagement.domain.user;

namespace SchoolManagement.domain.SchoolManagement.dto
{
    public class UserReadDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public Status Status { get; set; }
        public Role? Role { get; set; }
        public string? ClassroomId { get; set; }
        public List<CourseReadDto> Courses { get; set; } = new();
    }
}
