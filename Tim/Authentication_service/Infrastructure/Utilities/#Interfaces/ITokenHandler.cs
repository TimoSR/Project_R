using System.Security.Claims;

namespace Infrastructure.Utilities._Interfaces;

public interface ITokenHandler : IUtilityTool
{
    string GenerateToken(string userId);
    ClaimsPrincipal? DecodeToken(string token);
    string GenerateRefreshToken();
}