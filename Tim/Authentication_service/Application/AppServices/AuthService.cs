using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Registrations._Interfaces;
using Domain.DomainModels;
using Domain.IRepositories;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.AppServices
{
    public class AuthService : IAppService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;

        public AuthService(IUserRepository userRepository, IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<(User, string AccessToken, string RefreshToken)> AuthenticateGoogleUserAsync(string googleToken)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(googleToken);
            var email = payload.Email;

            var user = await _userRepository.FindByEmailAsync(email);

            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    GoogleId = payload.Subject,
                };

                await _userRepository.CreateUserAsync(user);
            }

            var (accessToken, refreshToken) = GenerateJwtToken(user);
            await SaveRefreshTokenToDatabase(user.Id, refreshToken);
            return (user, accessToken, refreshToken);
        }

        private (string AccessToken, string RefreshToken) GenerateJwtToken(User user)
        {
            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            return (accessToken, refreshToken);
        }

        private string GenerateAccessToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                }),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        private async Task SaveRefreshTokenToDatabase(string userId, string refreshToken)
        {
            var expiryTime = DateTime.UtcNow.AddHours(1);  // Set your own expiry time
            await _userRepository.UpdateRefreshTokenAsync(userId, refreshToken, expiryTime);
        }
    }
}
