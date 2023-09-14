namespace x_endpoints.Infrastructure.Helpers.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TopicNameAttribute : Attribute
{
    public string Name { get; }

    public TopicNameAttribute(string name)
    {
        Name = name;
    }
}
