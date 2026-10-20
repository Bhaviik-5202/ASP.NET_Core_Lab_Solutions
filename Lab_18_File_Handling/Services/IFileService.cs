using Microsoft.AspNetCore.Http;

namespace Lab_18_File_Handling.Services
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string subFolder);
        void DeleteFile(string? relativePath);
        bool FileExists(string? relativePath);
        string GetPhysicalPath(string? relativePath);
    }
}
