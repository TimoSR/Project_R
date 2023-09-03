namespace x_endpoints.Tools.Environment;

public interface IEnvironmentService : IApplicationTool
{
    string GetString(string key);
}