namespace Lab_15_17_API_Fundamentals.DTOs
{
    public class StudentFilterParameters
    {
        public string? Department { get; set; }
        public int? MinScore { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
