namespace Infrastructure.Utilities._Interfaces;

public interface ITokenGenerator : IUtilityTool
{
    string GenerateToken(string userId);
}