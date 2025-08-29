using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.domain.SchoolManagement.dto
{
    public class ClassRoomCreateDto
    {
        [Required] public string Name { get; set; }
        public string? Description { get; set; }
        public int TeacherId { get; set; }

    }
}
