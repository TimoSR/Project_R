namespace Infrastructure.Utilities._Interfaces;

public interface IJsonSerializer : IUtilityTool
{
    string Serialize<TData>(TData content);
}