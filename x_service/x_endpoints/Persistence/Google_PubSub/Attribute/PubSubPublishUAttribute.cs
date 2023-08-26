namespace x_endpoints.Persistence.Google_PubSub.Attribute;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
sealed class PubSubPublishUAttribute : System.Attribute
{
    public string ServiceName { get; }
    public string TopicName { get; }
    public string PayloadArgumentName { get; }  // Name of the action argument to use as payload.

    public PubSubPublishUAttribute(string topicName, string payloadArgumentName = "")
    {
        ServiceName = DotNetEnv.Env.GetString("SERVICE_NAME");
        TopicName = topicName;
        PayloadArgumentName = payloadArgumentName;
    }
}
