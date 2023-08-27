namespace x_endpoints.Tools.Serializers.Types;

public interface IMessage<TPayload>
{
    TPayload Content { get; set; }
}