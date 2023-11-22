using Domain.User.Entities;

namespace Infrastructure.Utilities._Interfaces;

public interface IPasswordHasher : IUtilityTool
{
    string HashPassword(User user);
    bool VerifyHashedPassword(User user, string password);
}