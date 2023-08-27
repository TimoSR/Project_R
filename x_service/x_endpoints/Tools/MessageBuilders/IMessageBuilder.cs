namespace x_endpoints.Tools.MessageBuilders;

public interface IMessageBuilder<T>
{
    T BuildMessage(string content);
}