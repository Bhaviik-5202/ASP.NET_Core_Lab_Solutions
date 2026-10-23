namespace Lab_20_21_Validation.DTOs
{
    public class StudentBasicDTO
    {
        public int StudentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string EnrollmentNumber { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
    }
}
