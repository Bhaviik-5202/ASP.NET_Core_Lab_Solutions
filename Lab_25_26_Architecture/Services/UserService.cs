using Lab_25_26_Architecture.DTOs;
using Lab_25_26_Architecture.Models;
using Lab_25_26_Architecture.Repositories;

namespace Lab_25_26_Architecture.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToResponseDto);
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : MapToResponseDto(user);
        }

        public async Task<string> AddAsync(UserDto dto)
        {
            var user = new User
            {
                UserTypeID = dto.UserTypeID,
                FullName = dto.FullName,
                UserCode = dto.UserCode,
                Email = dto.Email,
                Password = dto.Password,
                MobileNumber = dto.MobileNumber,
                ProfilePicturePath = dto.ProfilePicturePath,
                IsActive = true,
                IsDeleted = false
            };

            await _userRepository.AddAsync(user);
            return "Record Inserted";
        }

        public async Task<bool> UpdateAsync(int id, UserDto dto)
        {
            var existing = await _userRepository.GetByIdAsync(id);
            if (existing == null)
            {
                return false;
            }

            existing.UserTypeID = dto.UserTypeID;
            existing.FullName = dto.FullName;
            existing.UserCode = dto.UserCode;
            existing.Email = dto.Email;
            existing.Password = dto.Password;
            existing.MobileNumber = dto.MobileNumber;
            existing.ProfilePicturePath = dto.ProfilePicturePath;

            await _userRepository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        private static UserResponseDto MapToResponseDto(User user)
        {
            return new UserResponseDto
            {
                UserId = user.UserId,
                UserTypeID = user.UserTypeID,
                UserTypeName = user.UserTypeID switch
                {
                    1 => "Admin",
                    2 => "Faculty",
                    3 => "Student",
                    _ => "Unknown"
                },
                FullName = user.FullName,
                UserCode = user.UserCode,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                ProfilePicturePath = user.ProfilePicturePath,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
