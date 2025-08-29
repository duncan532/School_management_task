using SchoolManagement.domain.school_mgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.domain.user
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "firstname is required")]
        public string FirstName { get; set; } = default!;
        [Required(ErrorMessage = "Lastname is required")]
        public string LastName { get; set; } = default!;
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format.")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Age number is required")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Password number is required")]
        public string PasswordHash { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        public string Phone { get; set; } = default!;
        public Status Status { get; set; } = Status.Active;
        public Role? Role { get; set; } 
        public List<Course> Courses { get; set; } = new();
        public int? ClassroomId { get; set; }
        public Classroom? Classroom { get; set; }
        public List<Enrollment> Enrollments { get; set; } = new();
    }
}
