using System.Security.Claims;
using _CommonLibrary.Patterns.RegistrationHooks.Utilities;
using Domain._Registration;

namespace Infrastructure.Utilities._Interfaces;

public interface ITokenHandler : IUtilityTool
{
    string GenerateJwtToken(string userId);
    ClaimsPrincipal? DecodeJwtToken(string token);
    string GenerateRefreshToken();
}