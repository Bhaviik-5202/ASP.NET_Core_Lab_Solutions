namespace Lab_25_26_Architecture.Models
{
    public class User
    {
        public int UserId { get; set; }
        public int UserTypeID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string UserCode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string? ProfilePicturePath { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
