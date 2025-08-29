namespace SchoolManagement.domain.SchoolManagement.dto
{
    public class CourseReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public UserReadDto Teacher { get; set; }
        public List<EnrollmentReadDto> EnrolledStudents { get; set; } = new();
    }
}
