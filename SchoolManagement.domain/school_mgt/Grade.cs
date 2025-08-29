namespace SchoolManagement.domain.school_mgt
{
    public class Grade
    {
        public int Id { get; set; }
        public double Score { get; set; }
        public int EnrollmentId { get; set; }   
        public Enrollment? Enrollment { get; set; }
    }

}
