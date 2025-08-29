using System.ComponentModel.DataAnnotations;



namespace SchoolManagement.domain
{
    public class Classroom
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public List<Course> Courses { get; set; } = new();
    }

}
