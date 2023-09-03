namespace x_endpoints.Tools.Environment;

public class DotNetEnvWrapper : IEnvironmentService
{
    public string GetString(string key)
    {
        return DotNetEnv.Env.GetString(key);
    }
}