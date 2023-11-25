using _CommonLibrary.Patterns.RegistrationHooks.Utilities;
using Domain._Registration;

namespace Infrastructure.Utilities._Interfaces;

public interface IJsonSerializer : IUtilityTool
{
    string Serialize<TData>(TData content);
}