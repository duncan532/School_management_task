using SchoolManagement.domain.user;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.domain.SchoolManagement.dto
{
    public class UserCreateDto
    {
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required][EmailAddress] public string Email { get; set; }
        [Required] public int Age { get; set; }
        [Required] public string Phone { get; set; }
        [Required] public string Password { get; set; }
        public Role? Role { get; set; }
        public int? ClassroomId { get; set; }
    }
}
