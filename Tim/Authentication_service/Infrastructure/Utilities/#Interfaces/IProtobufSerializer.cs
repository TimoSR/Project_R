using _CommonLibrary.Patterns.RegistrationHooks.Utilities;
using Domain._Registration;

namespace Infrastructure.Utilities._Interfaces;

public interface IProtobufSerializer : IUtilityTool
{
    string Serialize<TData>(TData content);
    TModel Deserialize<TModel>(string content);
}