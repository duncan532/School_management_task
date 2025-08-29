namespace SchoolManagement.domain
{
    public class Student
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; } 
        public Status Status { get; set; } = Status.Active;
        public int ClassroomId { get; set; }
        public List<Enrollment> Enrollments { get; set; } = new();
    }

}
