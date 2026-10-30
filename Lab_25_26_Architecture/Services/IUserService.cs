using Lab_25_26_Architecture.DTOs;

namespace Lab_25_26_Architecture.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto?> GetUserByIdAsync(int id);
        Task<string> AddAsync(UserDto dto);
        Task<bool> UpdateAsync(int id, UserDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
