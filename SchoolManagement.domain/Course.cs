namespace SchoolManagement.domain
{
    public class Course
    {
        public int Id { get; set; }
        public string ?Name { get; set; }
        public string? Description { get; set; }
        public Teacher? Teacher { get; set; }
        public int? TeacherId { get; set; }
        public Classroom? Classroom { get; set; }
        public int ClassroomId { get; set; }
        public List<Enrollment> Enrollments { get; set; } = new();
    }

}