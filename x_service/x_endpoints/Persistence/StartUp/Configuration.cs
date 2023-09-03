using System.Collections;

namespace x_endpoints.Persistence.StartUp;

public class Configuration
{
    public string ProjectId { get; set; }
    public string ServiceName { get; set; }
    public string Environment { get; set; }
    public IDictionary EnvironmentVariables { get; set; }
    public string ConnectionString { get; set; }
}