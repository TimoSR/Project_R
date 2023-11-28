using _CommonLibrary.Patterns.RegistrationHooks.Utilities;
using Domain._Registration;

namespace Infrastructure.Utilities._Interfaces;

public interface IPasswordHasher : IUtilityTool
{
    string HashPassword(string password);
    bool VerifyHashedPassword(string hashedPassword, string providedPassword);
}