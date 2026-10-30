using Lab_25_26_Architecture.Models;

namespace Lab_25_26_Architecture.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static readonly List<User> _users = new()
        {
            new User
            {
                UserId = 1,
                UserTypeID = 1,
                FullName = "Madhuresh Shah",
                UserCode = "ADM001",
                Email = "admin@college.edu",
                Password = "Password@123",
                MobileNumber = "9876543210",
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                UserId = 2,
                UserTypeID = 3,
                FullName = "Bhavik Parmar",
                UserCode = "STD121",
                Email = "bhavik@college.edu",
                Password = "Password@123",
                MobileNumber = "9123456780",
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            }
        };

        public Task<IEnumerable<User>> GetAllAsync()
        {
            var activeUsers = _users.Where(u => !u.IsDeleted).AsEnumerable();
            return Task.FromResult(activeUsers);
        }

        public Task<User?> GetByIdAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
            return Task.FromResult(user);
        }

        public Task AddAsync(User user)
        {
            user.UserId = _users.Count > 0 ? _users.Max(u => u.UserId) + 1 : 1;
            user.CreatedAt = DateTime.UtcNow;
            user.IsActive = true;
            user.IsDeleted = false;

            _users.Add(user);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(User user)
        {
            var existing = _users.FirstOrDefault(u => u.UserId == user.UserId);
            if (existing != null)
            {
                existing.FullName = user.FullName;
                existing.Email = user.Email;
                existing.Password = user.Password;
                existing.MobileNumber = user.MobileNumber;
                existing.UserTypeID = user.UserTypeID;
                existing.UserCode = user.UserCode;
                existing.ProfilePicturePath = user.ProfilePicturePath;
            }
            return Task.CompletedTask;
        }

        public Task<bool> DeleteAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
            if (user == null)
            {
                return Task.FromResult(false);
            }

            user.IsDeleted = true;
            return Task.FromResult(true);
        }
    }
}
