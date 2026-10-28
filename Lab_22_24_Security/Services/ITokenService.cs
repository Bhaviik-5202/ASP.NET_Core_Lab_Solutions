using System.Security.Claims;
using Lab_22_24_Security.Models;

namespace Lab_22_24_Security.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(UserModel user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
