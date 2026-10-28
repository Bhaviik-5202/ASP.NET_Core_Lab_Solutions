using System.Security.Claims;
using Lab_22_24_Security.DTOs;
using Lab_22_24_Security.Models;
using Lab_22_24_Security.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab_22_24_Security.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        private static readonly List<UserModel> _users = new()
        {
            new UserModel { UserId = 1, UserName = "admin", Email = "admin@college.edu", Password = "Admin@123", Role = "Admin" },
            new UserModel { UserId = 2, UserName = "faculty", Email = "faculty@college.edu", Password = "Faculty@123", Role = "Faculty" },
            new UserModel { UserId = 3, UserName = "student", Email = "student@college.edu", Password = "Student@123", Role = "Student" }
        };

        public AuthController(ITokenService tokenService, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto? dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new { message = "Username and password are required." });
            }

            var user = _users.FirstOrDefault(u =>
                u.UserName.Equals(dto.UserName, StringComparison.OrdinalIgnoreCase) &&
                u.Password == dto.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var refreshExpiryDays = int.TryParse(_configuration["Jwt:RefreshTokenExpiresInDays"], out var days) ? days : 7;
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshExpiryDays);

            var expiryMinutes = int.TryParse(_configuration["Jwt:ExpiresInMinutes"], out var exp) ? exp : 15;

            return Ok(new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                TokenType = "Bearer",
                ExpiresInMinutes = expiryMinutes,
                UserName = user.UserName,
                Role = user.Role
            });
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshTokenRequestDto? dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.AccessToken) || string.IsNullOrWhiteSpace(dto.RefreshToken))
            {
                return BadRequest(new { message = "Invalid client request: AccessToken and RefreshToken are required." });
            }

            var principal = _tokenService.GetPrincipalFromExpiredToken(dto.AccessToken);
            if (principal == null)
            {
                return BadRequest(new { message = "Invalid access token." });
            }

            var username = principal.Identity?.Name;
            var user = _users.FirstOrDefault(u => u.UserName == username);

            if (user == null || string.IsNullOrEmpty(user.RefreshToken) || user.RefreshToken != dto.RefreshToken || user.RefreshTokenExpiryTime == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return BadRequest(new { message = "Invalid or expired refresh token. Please login again." });
            }

            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            var refreshExpiryDays = int.TryParse(_configuration["Jwt:RefreshTokenExpiresInDays"], out var days) ? days : 7;
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshExpiryDays);

            var expiryMinutes = int.TryParse(_configuration["Jwt:ExpiresInMinutes"], out var exp) ? exp : 15;

            return Ok(new TokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                TokenType = "Bearer",
                ExpiresInMinutes = expiryMinutes,
                UserName = user.UserName,
                Role = user.Role
            });
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var username = User.Identity?.Name;
            var user = _users.FirstOrDefault(u => u.UserName == username);

            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
            }

            return Ok(new { message = "Logged out successfully. Refresh token invalidated." });
        }

        [AllowAnonymous]
        [HttpGet("public")]
        public IActionResult PublicEndpoint()
        {
            return Ok(new { message = "This is a public endpoint accessible without authentication." });
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetUserProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.Identity?.Name;
            var email = User.FindFirstValue(ClaimTypes.Email);
            var role = User.FindFirstValue(ClaimTypes.Role);

            return Ok(new
            {
                message = "Authenticated Profile Data",
                userId = userId,
                userName = userName,
                email = email,
                role = role
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnly()
        {
            return Ok(new
            {
                message = "Access Granted: Welcome Administrator!",
                authorizedUser = User.Identity?.Name,
                role = "Admin"
            });
        }

        [Authorize(Roles = "Faculty,Student")]
        [HttpGet("faculty-student")]
        public IActionResult FacultyAndStudentArea()
        {
            return Ok(new
            {
                message = "Access Granted: Welcome to the Academic Portal.",
                authorizedUser = User.Identity?.Name,
                userRole = User.FindFirstValue(ClaimTypes.Role)
            });
        }

        [Authorize(Policy = "MustBeAdmin")]
        [HttpGet("policy-admin")]
        public IActionResult PolicyBasedAdmin()
        {
            return Ok(new
            {
                message = "Access Granted via Policy 'MustBeAdmin'.",
                currentUser = User.Identity?.Name
            });
        }
    }
}
