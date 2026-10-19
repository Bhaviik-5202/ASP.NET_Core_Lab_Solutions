namespace Lab_15_17_API_Fundamentals.DTOs
{
    public class StudentDto
    {
        public int StudentId { get; set; }
        public Guid ExternalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string EnrollmentNumber { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public int Score { get; set; }
        public string CourseCode { get; set; } = string.Empty;
    }

    public class CreateStudentDto
    {
        public string Name { get; set; } = string.Empty;
        public string EnrollmentNumber { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public int Score { get; set; }
        public string CourseCode { get; set; } = string.Empty;
    }
}
