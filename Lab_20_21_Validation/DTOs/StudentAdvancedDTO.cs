using Lab_20_21_Validation.Enums;

namespace Lab_20_21_Validation.DTOs
{
    public class StudentAdvancedDTO
    {
        public int StudentId { get; set; }
        public string EnrollmentNo { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public int DepartmentId { get; set; }
        public int Semester { get; set; }
        public bool IsHosteller { get; set; }
        public string? HostelName { get; set; }
        public decimal Fees { get; set; }
        public List<CourseDTO> Courses { get; set; } = new();
    }
}
