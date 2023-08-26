using Newtonsoft.Json;

namespace x_endpoints.Persistence.Google_PubSub.PubEvents;

public class JsonEventMessageService : IEventMessageService
{
    public string CreateMessage(object payload)
    {
        return JsonConvert.SerializeObject(payload);
    }
}