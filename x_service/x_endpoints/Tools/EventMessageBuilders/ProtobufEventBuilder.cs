using x_endpoints.Tools.EventMessageBuilders.Types;

namespace x_endpoints.Tools.EventMessageBuilders;

public class ProtobufEventBuilder : IEventBuilder<ProtobufMessage>, ITool
{
    public ProtobufMessage BuildMessage(string content)
    {
        return new ProtobufMessage { Content = System.Text.Encoding.UTF8.GetBytes(content) };
    }
}