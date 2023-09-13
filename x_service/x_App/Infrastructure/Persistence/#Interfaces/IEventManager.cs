namespace x_endpoints.Infrastructure.Persistence._Interfaces;

public interface IEventManager
{
    Task PublishJsonEventAsync<TEvent>(TEvent eventMessage);
    Task PublishProtobufEventAsync<TEvent>(TEvent eventMessage);
}