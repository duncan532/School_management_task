using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.domain.SchoolManagement.dto
{
    public class CourseCreateDto
    {
        [Required] public string Name { get; set; }
        public string? Description { get; set; }
        public int TeacherId { get; set; }
    }
}
