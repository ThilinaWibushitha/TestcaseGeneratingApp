using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examples
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<List<User>> GetAllAsync();
        Task<User> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }

    public interface IEmailService
    {
        Task SendWelcomeEmailAsync(string email, string name);
        Task SendNotificationAsync(string email, string message);
    }

    /// <summary>
    /// Example service class with dependencies for demonstrating test generation
    /// </summary>
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public UserService(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task<User?> GetUserAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("User ID must be positive", nameof(id));

            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<List<User>> GetActiveUsersAsync()
        {
            var allUsers = await _userRepository.GetAllAsync();
            return allUsers.Where(u => u.IsActive).ToList();
        }

        public async Task<User> CreateUserAsync(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));

            if (!IsValidEmail(email))
                throw new ArgumentException("Invalid email format", nameof(email));

            var user = new User
            {
                Name = name,
                Email = email,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userRepository.CreateAsync(user);
            await _emailService.SendWelcomeEmailAsync(email, name);

            return createdUser;
        }

        public async Task DeactivateUserAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("User ID must be positive", nameof(id));

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new InvalidOperationException($"User with ID {id} not found");

            user.IsActive = false;
            await _userRepository.UpdateAsync(user);
            await _emailService.SendNotificationAsync(user.Email, "Your account has been deactivated");
        }

        public async Task<bool> UserExistsAsync(int id)
        {
            if (id <= 0)
                return false;

            return await _userRepository.ExistsAsync(id);
        }

        public async Task UpdateUserEmailAsync(int id, string newEmail)
        {
            if (id <= 0)
                throw new ArgumentException("User ID must be positive", nameof(id));

            if (string.IsNullOrWhiteSpace(newEmail))
                throw new ArgumentException("Email cannot be empty", nameof(newEmail));

            if (!IsValidEmail(newEmail))
                throw new ArgumentException("Invalid email format", nameof(newEmail));

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new InvalidOperationException($"User with ID {id} not found");

            user.Email = newEmail;
            await _userRepository.UpdateAsync(user);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
