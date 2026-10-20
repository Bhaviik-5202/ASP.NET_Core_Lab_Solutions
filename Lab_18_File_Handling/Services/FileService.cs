using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Lab_18_File_Handling.Services
{
    public class FileService : IFileService
    {
        private readonly string _webRootPath;
        private const string FilesBaseFolder = "Files";

        public FileService(IWebHostEnvironment environment)
        {
            _webRootPath = environment.WebRootPath
                ?? Path.Combine(environment.ContentRootPath, "wwwroot");

            if (!Directory.Exists(_webRootPath))
            {
                Directory.CreateDirectory(_webRootPath);
            }
        }

        public async Task<string> UploadFileAsync(IFormFile file, string subFolder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty or not provided.");

            string uploadFolderPath = Path.Combine(_webRootPath, FilesBaseFolder, subFolder);

            if (!Directory.Exists(uploadFolderPath))
            {
                Directory.CreateDirectory(uploadFolderPath);
            }

            string safeFileName = Path.GetFileName(file.FileName);
            string uniqueFileName = $"{Guid.NewGuid()}_{safeFileName}";
            string fullPhysicalPath = Path.Combine(uploadFolderPath, uniqueFileName);

            using (var stream = new FileStream(fullPhysicalPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine(FilesBaseFolder, subFolder, uniqueFileName).Replace("\\", "/");
        }

        public void DeleteFile(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return;

            string fullPath = GetPhysicalPath(relativePath);

            if (File.Exists(fullPath))
            {
                try
                {
                    File.Delete(fullPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting physical file at {fullPath}: {ex.Message}");
                }
            }
        }

        public bool FileExists(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return false;
            string fullPath = GetPhysicalPath(relativePath);
            return File.Exists(fullPath);
        }

        public string GetPhysicalPath(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return string.Empty;
            return Path.Combine(_webRootPath, relativePath.TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar));
        }
    }
}
