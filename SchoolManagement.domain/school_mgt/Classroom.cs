using SchoolManagement.domain.user;
using System.ComponentModel.DataAnnotations;



namespace SchoolManagement.domain.school_mgt
{
    public class Classroom
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int TeacherId { get; set; }
        public User? Teacher { get; set; }
    }

}
