namespace CommonLibrary.Infrastructure.Utilities._Interfaces;

public interface IEmailValidator : IUtilityTool
{
    bool IsValid(string email);
}