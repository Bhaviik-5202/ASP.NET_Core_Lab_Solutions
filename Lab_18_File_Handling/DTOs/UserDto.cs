using Microsoft.AspNetCore.Http;

namespace Lab_18_File_Handling.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public IFormFile? DocumentFile { get; set; }
    }

    public class UserResponseDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? DocumentPath { get; set; }
        public string? FileAccessUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
