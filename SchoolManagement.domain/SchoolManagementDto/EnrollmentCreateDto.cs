using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.domain.SchoolManagement.dto
{
    public class EnrollmentCreateDto
    {
        [Required] public int StudentId { get; set; }
        [Required] public int CourseId { get; set; }

    }
}
