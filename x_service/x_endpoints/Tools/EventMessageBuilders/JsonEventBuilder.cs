using Newtonsoft.Json;
using x_endpoints.Tools.EventMessageBuilders.Types;

namespace x_endpoints.Tools.EventMessageBuilders;

public class JsonEventBuilder : IEventBuilder<JsonMessage>, ITool
{
    public JsonMessage BuildMessage(string content)
    {
        return new JsonMessage { Content = content };
    }
}