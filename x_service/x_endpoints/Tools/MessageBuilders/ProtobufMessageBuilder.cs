using x_endpoints.Tools.EventMessageBuilders.Types;

namespace x_endpoints.Tools.MessageBuilders;

public class ProtobufMessageBuilder : IMessageBuilder<ProtobufMessage>, IApplicationTool
{
    public ProtobufMessage BuildMessage(string content)
    {
        return new ProtobufMessage { Content = System.Text.Encoding.UTF8.GetBytes(content) };
    }
}