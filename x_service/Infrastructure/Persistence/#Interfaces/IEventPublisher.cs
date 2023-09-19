namespace Infrastructure.Persistence._Interfaces;

public interface IEventPublisher
{
    Task PublishJsonEventAsync<TEvent>(TEvent eventMessage);
    Task PublishProtobufEventAsync<TEvent>(TEvent eventMessage);
}