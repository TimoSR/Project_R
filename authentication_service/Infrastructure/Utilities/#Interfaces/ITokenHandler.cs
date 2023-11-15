using System.Security.Claims;

namespace Infrastructure.Utilities._Interfaces;

public interface ITokenHandler : IUtilityTool
{
    string GenerateJwtToken(string userId);
    ClaimsPrincipal? DecodeJwtToken(string token);
    string GenerateRefreshToken();
}