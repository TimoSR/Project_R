using System.Reflection;
using x_endpoints.Registration.Services;
using x_endpoints.Tools;

namespace x_endpoints.Registration.Tools;

public static class ToolRegistration
{
    public static IServiceCollection AddApplicationTools(this IServiceCollection services)
    {   
        
        // Using reflection to get all types which are classes, not abstract, and implement ITool
        var toolTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(ITool).IsAssignableFrom(t));

        foreach (var type in toolTypes)
        {
            services.AddTransient(type);
        }

        return services;
    }
}