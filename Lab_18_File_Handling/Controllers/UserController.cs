using Lab_18_File_Handling.DTOs;
using Lab_18_File_Handling.Models;
using Lab_18_File_Handling.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab_18_File_Handling.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IFileService _fileService;

        private static readonly List<UserModel> _users = new()
        {
            new UserModel { UserId = 1, UserName = "Bhavik", Password = "Password123", DocumentPath = null, CreatedAt = DateTime.UtcNow }
        };

        public UserController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var response = _users.Select(u => MapToResponse(u, baseUrl)).ToList();
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var user = _users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
                return NotFound(new { message = $"User with ID {id} was not found." });

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            return Ok(MapToResponse(user, baseUrl));
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] UserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName))
                return BadRequest(new { message = "UserName is required." });

            string? uploadedPath = null;
            if (dto.DocumentFile != null && dto.DocumentFile.Length > 0)
            {
                const long maxFileSize = 15 * 1024 * 1024;
                if (dto.DocumentFile.Length > maxFileSize)
                    return BadRequest(new { message = "File size exceeds the 15 MB limit." });

                uploadedPath = await _fileService.UploadFileAsync(dto.DocumentFile, "Users");
            }

            var nextId = _users.Count > 0 ? _users.Max(u => u.UserId) + 1 : 1;
            var user = new UserModel
            {
                UserId = nextId,
                UserName = dto.UserName,
                Password = dto.Password,
                DocumentPath = uploadedPath,
                CreatedAt = DateTime.UtcNow
            };

            _users.Add(user);

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            return CreatedAtAction(nameof(GetById), new { id = user.UserId }, MapToResponse(user, baseUrl));
        }

        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] UserDto dto)
        {
            var existingUser = _users.FirstOrDefault(u => u.UserId == id);
            if (existingUser == null)
                return NotFound(new { message = $"User with ID {id} was not found." });

            if (dto.DocumentFile != null && dto.DocumentFile.Length > 0)
            {
                const long maxFileSize = 15 * 1024 * 1024;
                if (dto.DocumentFile.Length > maxFileSize)
                    return BadRequest(new { message = "File size exceeds the 15 MB limit." });

                if (!string.IsNullOrWhiteSpace(existingUser.DocumentPath))
                {
                    _fileService.DeleteFile(existingUser.DocumentPath);
                }

                existingUser.DocumentPath = await _fileService.UploadFileAsync(dto.DocumentFile, "Users");
            }

            if (!string.IsNullOrWhiteSpace(dto.UserName))
            {
                existingUser.UserName = dto.UserName;
            }

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                existingUser.Password = dto.Password;
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            return Ok(new
            {
                message = "User updated successfully.",
                user = MapToResponse(existingUser, baseUrl)
            });
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id, [FromQuery] bool deleteFileOnly = false)
        {
            var user = _users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
                return NotFound(new { message = $"User with ID {id} was not found." });

            if (deleteFileOnly)
            {
                if (string.IsNullOrEmpty(user.DocumentPath))
                    return BadRequest(new { message = "No document exists for this user." });

                _fileService.DeleteFile(user.DocumentPath);
                user.DocumentPath = null;

                return Ok(new { message = "Document deleted successfully. User record preserved." });
            }

            if (!string.IsNullOrEmpty(user.DocumentPath))
            {
                _fileService.DeleteFile(user.DocumentPath);
            }

            _users.Remove(user);
            return NoContent();
        }

        [HttpGet("{id:int}/download")]
        public IActionResult DownloadFile(int id)
        {
            var user = _users.FirstOrDefault(u => u.UserId == id);
            if (user == null || string.IsNullOrWhiteSpace(user.DocumentPath))
                return NotFound(new { message = "No document found for this user." });

            var physicalPath = _fileService.GetPhysicalPath(user.DocumentPath);
            if (!System.IO.File.Exists(physicalPath))
                return NotFound(new { message = "Physical file is missing from server disk." });

            var fileStream = new FileStream(physicalPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var fileName = Path.GetFileName(physicalPath);

            return File(fileStream, "application/octet-stream", fileName);
        }

        private static UserResponseDto MapToResponse(UserModel u, string baseUrl) => new()
        {
            UserId = u.UserId,
            UserName = u.UserName,
            DocumentPath = u.DocumentPath,
            FileAccessUrl = !string.IsNullOrWhiteSpace(u.DocumentPath)
                ? $"{baseUrl}/{u.DocumentPath}"
                : null,
            CreatedAt = u.CreatedAt
        };
    }
}
