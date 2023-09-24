using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Infrastructure.Utilities._Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Utilities.TokenGenerator;

public class TokenHandler : ITokenHandler
{
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly ILogger<TokenHandler> _logger;

    public TokenHandler(IConfiguration configuration, ILogger<TokenHandler> logger = null)
    {
        _key = configuration.JwtKey;
        _issuer = configuration.JwtIssuer;
        _audience = configuration.JwtAudience;
        _logger = logger;
    }

    public string GenerateToken(string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Convert.FromBase64String(_key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", userId) }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal DecodeToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(_key);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ClockSkew = TimeSpan.Zero
            };

            return tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
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
            _logger.LogError($"An error occured while decoding the token: {ex.Message}");
            throw;
        }
    }
}