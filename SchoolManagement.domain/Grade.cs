namespace SchoolManagement.domain
{
    public class Grade
    {
        public int Id { get; set; }
        public int EnrollmentId { get; set; }
        public double Score { get; set; }
        public Enrollment? Enrollment { get; set; }
    }

}
