using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Registrations._Interfaces;
using Domain.DomainModels;
using Infrastructure.DomainRepositories;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Application.AppServices
{
    public class AuthService : IAppService  // Assuming IAuthService is your service interface
    {
        private readonly UserRepository _userRepository;
        private readonly string _key;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(UserRepository userRepository, Configuration configuration)
        {
            _userRepository = userRepository;
            _key = configuration.JwtKey;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<string> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.FindByEmailAsync(email);

            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Password, password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(_key); // Replace with your key
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> RegisterAsync(User newUser)
        {
            // Check if email already exists
            var existingUser = await _userRepository.FindByEmailAsync(newUser.Email);

            if (existingUser != null)
            {
                return false;
            }

            // Hash the password
            newUser.Password = _passwordHasher.HashPassword(newUser, newUser.Password);

            // Insert new user
            await _userRepository.CreateUserAsync(newUser);
            return true;
        }


        public async Task<bool> LogoutAsync(string userId)
        {
            // Invalidate the refresh token (optional)
            await _userRepository.UpdateRefreshTokenAsync(userId, null, DateTime.UtcNow);
            return true;
        }
        
        public async Task<bool> DeleteUserAsync(string userId)
        {
            // Deleting the user (pseudo code, implement actual delete logic)
            await _userRepository.DeleteUserAsync(userId);
            return true;
        }
    }
}
