namespace SchoolManagement.domain.SchoolManagement.dto
{
    public class CurrentUserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = default!;
        public string Role { get; set; } = default!;
    }
}
