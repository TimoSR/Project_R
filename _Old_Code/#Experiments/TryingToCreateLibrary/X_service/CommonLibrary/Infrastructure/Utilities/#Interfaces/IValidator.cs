namespace CommonLibrary.Infrastructure.Utilities._Interfaces;

public interface IPasswordValidator : IUtilityTool
{
    bool IsValid(string password);
}