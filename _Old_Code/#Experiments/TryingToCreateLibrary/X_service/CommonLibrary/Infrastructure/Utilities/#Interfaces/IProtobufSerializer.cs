namespace CommonLibrary.Infrastructure.Utilities._Interfaces;

public interface IProtobufSerializer : IUtilityTool
{
    string Serialize<TData>(TData content);
}