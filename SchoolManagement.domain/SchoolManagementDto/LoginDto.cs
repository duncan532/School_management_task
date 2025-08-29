namespace SchoolManagement.domain.SchoolManagement.dto
{
    public class LoginDto
    {
        public required string Email { get; set; } = default!;
        public required string Password { get; set; } = default!;
    }
}
