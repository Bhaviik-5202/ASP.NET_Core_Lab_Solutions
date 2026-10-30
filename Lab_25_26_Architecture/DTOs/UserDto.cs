namespace Lab_25_26_Architecture.DTOs
{
    public class UserDto
    {
        public int UserTypeID { get; set; } = 3;
        public string FullName { get; set; } = string.Empty;
        public string UserCode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string? ProfilePicturePath { get; set; }
    }

    public class UserResponseDto
    {
        public int UserId { get; set; }
        public int UserTypeID { get; set; }
        public string UserTypeName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string UserCode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string? ProfilePicturePath { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
