using System.Collections;

namespace Infrastructure.Persistence._Interfaces;

public class Configuration
{
    public string HostUrl { get; set; }
    public string ProjectId { get; set; }
    public string ServiceName { get; set; }
    public string Environment { get; set; }
    public IDictionary EnvironmentVariables { get; set; }
    public string MongoConnectionString { get; set; }
    public string RedisConnectionString { get; set; }
}