using Domain.DomainModels;
using Infrastructure.Utilities._Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Utilities.Password;

public class PasswordHasher : IUtilityTool
{
    private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

    public string HashPassword(User user)
    {
        return _passwordHasher.HashPassword(user, user.Password);
    }

    public bool VerifyHashedPassword(User user, string password)
    {
        return _passwordHasher.VerifyHashedPassword(user, user.Password, password) != PasswordVerificationResult.Failed;
    }
}