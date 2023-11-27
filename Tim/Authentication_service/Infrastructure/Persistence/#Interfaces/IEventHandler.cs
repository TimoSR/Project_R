namespace Infrastructure.Persistence._Interfaces;

public interface IEventHandler
{
    Task PublishJsonEventAsync<TEvent>(TEvent eventMessage);
    Task PublishProtobufEventAsync<TEvent>(TEvent eventMessage);
    TEvent? ProcessReceivedEvent<TEvent>(string receivedEvent) where TEvent : class;
}