using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Infrastructure.Utilities._Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Utilities.Token;

public class TokenHandler : ITokenHandler
{
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<TokenHandler> _logger;

    public TokenHandler(IConfiguration configuration, ILogger<TokenHandler> logger)
    {
        _jwtSettings = new JwtSettings()
        {
            Key = configuration.JwtKey,
            Issuer = configuration.JwtIssuer,
            Audience = configuration.JwtAudience,
            ExpirationInHours = 6
        };
       
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public string GenerateToken(string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Convert.FromBase64String(_jwtSettings.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", userId) }),
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationInHours),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal DecodeToken(string token)
    {
        try
        {
            var key = Convert.FromBase64String(_jwtSettings.Key);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ClockSkew = TimeSpan.Zero
            };

            return new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out _);
        }
        catch (SecurityTokenExpiredException ex)
        {
            _logger.LogWarning($"Token expired: {ex.Message}");
            throw;
        }
        catch (SecurityTokenInvalidSignatureException ex)
        {
            _logger.LogWarning($"Token has an invalid signature: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while decoding the token: {ex.Message}");
            throw;
        }
    }

    public string GenerateRefreshToken()
    {
        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[32];
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}

public class JwtSettings
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpirationInHours { get; set; }
}