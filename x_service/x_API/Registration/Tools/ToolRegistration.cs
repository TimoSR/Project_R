using System.Reflection;
using x_endpoints.Registration.Services;
using x_endpoints.Tools;

namespace x_endpoints.Registration.Tools;

public static class ToolRegistration
{
    public static IServiceCollection AddApplicationTools(this IServiceCollection services)
    {   
        
        // Using reflection to get all types which are classes, not abstract, and implement IApplicationTool
        var toolTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(ITool).IsAssignableFrom(t));

        foreach (var type in toolTypes)
        {
            var interfaceTypes = type.GetInterfaces().Where(i => typeof(ITool).IsAssignableFrom(i) && i != typeof(ITool)).ToList();

            if (interfaceTypes.Count == 0)
            {
                // If the type does not have any interface apart from IApplicationTool
                services.AddTransient(type);
            }
            else
            {
                // If the type implements a specific interface derived from IApplicationTool
                foreach (var interfaceType in interfaceTypes)
                {
                    services.AddTransient(interfaceType, type);
                }
            }
        }

        return services;
    }
}