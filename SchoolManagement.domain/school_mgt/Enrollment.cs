using SchoolManagement.domain.user;
namespace SchoolManagement.domain.school_mgt
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public User? Student { get; set; }
        public Course? Course { get; set; }
        public  Grade? Grade { get; set; }
    }

}
