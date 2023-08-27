using x_endpoints.Tools.EventMessageBuilders.Types;

namespace x_endpoints.Tools.MessageBuilders;

public class JsonMessageBuilder : IMessageBuilder<JsonMessage>, IApplicationTool
{
    public JsonMessage BuildMessage(string content)
    {
        return new JsonMessage { Content = content };
    }
}