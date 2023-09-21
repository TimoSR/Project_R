using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Registrations._Interfaces;
using Domain.DomainModels;
using Domain.IRepositories;
using Infrastructure.DomainRepositories;
using Infrastructure.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AppServices
{
    public class AuthService : IAppService  // Assuming IAuthService is your service interface
    {
        private readonly UserRepository _userRepository;
        private readonly string _key;

        public AuthService(UserRepository userRepository, Configuration configuration)
        {
            _userRepository = userRepository;
            _key = configuration.JwtKey;
        }

        public async Task<string> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.FindByEmailAsync(email);

            if (user == null || user.Password != password)  // Replace with password hash check in production
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
            // await _userRepository.DeleteUserAsync(userId);
            return true;
        }
    }
}
