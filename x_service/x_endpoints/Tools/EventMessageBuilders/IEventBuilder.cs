namespace x_endpoints.Tools.EventMessageBuilders;

public interface IEventBuilder<T>
{
    T BuildMessage(string content);
}