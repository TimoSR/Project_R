using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Infrastructure.Utilities._Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Utilities.TokenGenerator;

public class TokenGenerator : ITokenGenerator
{
    private readonly string _key;

    public TokenGenerator(IConfiguration configuration)
    {
        _key = configuration.JwtKey;
    }

    public string GenerateToken(string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Convert.FromBase64String(_key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", userId) }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}