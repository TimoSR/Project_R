namespace Domain.User.Repositories;

public interface IUserRepository
{
    // Read Operations
    Task<Entities.User> GetUserByIdAsync(string id);
    Task<IEnumerable<Entities.User>> GetAllUsersAsync();
    Task<Entities.User> FindByEmailAsync(string email);

    // Create Operation
    Task<bool> CreateUserIfNotRegisteredAsync(Entities.User newUser);

    // Update Operations
    Task UpdateUserAsync(Entities.User user);
    Task UpdateUserEmailAsync(string userId, string newEmail);
    Task UpdateUserPasswordAsync(string userId, string newPassword);

    // Delete Operation
    Task<bool> DeleteUserAsync(string id);
}