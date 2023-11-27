using Domain.UserManagement.Entities;

namespace Domain.UserManagement.Repositories;

public interface IUserRepository
{   
    // Read Operations
    Task<User> GetUserByIdAsync(string id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> FindByEmailAsync(string email);

    // Create Operation
    Task<bool> CreateUserIfNotRegisteredAsync(User newUser);

    // Update Operations
    Task UpdateUserAsync(User user);
    Task UpdateUserEmailAsync(string userId, string newEmail);
    Task UpdateUserPasswordAsync(string userId, string newPassword);

    // Delete Operation
    Task<bool> DeleteUserAsync(string id);
}