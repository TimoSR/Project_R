namespace x_endpoints.Tools.Serializers.Types;

public class JsonMessage<TPayload> : IMessage<TPayload>
{
    public TPayload Content { get; set; }
}