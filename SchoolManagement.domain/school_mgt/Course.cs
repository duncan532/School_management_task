using SchoolManagement.domain.user;
namespace SchoolManagement.domain.school_mgt
{
    public class Course
    {
        public int Id { get; set; }
        public string ?Name { get; set; }
        public string? Description { get; set; }
        public int? TeacherId { get; set; }
        public User? Teacher { get; set; }
        public List<Enrollment> Enrollments { get; set; } = new();
    }

}