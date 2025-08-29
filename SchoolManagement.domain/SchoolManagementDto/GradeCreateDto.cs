using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.domain.SchoolManagement.dto
{
    public class GradeCreateDto
    {
        [Required] public int EnrollmentId { get; set; }
        [Required] public double Score { get; set; }
    }
}
