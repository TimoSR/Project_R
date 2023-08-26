namespace x_endpoints.Persistence.Google_PubSub.PubEvents;

public interface IEventMessageService
{
    string CreateMessage(object payload);
}