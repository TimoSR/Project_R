namespace x_endpoints.Tools.Serializers;

public interface ISerializer<TPayload> : IApplicationTool
{
    string Serialize(TPayload content);
}