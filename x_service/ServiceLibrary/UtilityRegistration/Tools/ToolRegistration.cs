using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ServiceLibrary.Utilities._Interfaces;

namespace ServiceLibrary.UtilityRegistration.Tools;

public static class ToolRegistration
{
    public static IServiceCollection AddApplicationTools(this IServiceCollection services)
    {   
        
        // Using reflection to get all types which are classes, not abstract, and implement IApplicationTool
        var toolTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(IUtilityTool).IsAssignableFrom(t));

        foreach (var type in toolTypes)
        {
            var interfaceTypes = type.GetInterfaces().Where(i => typeof(IUtilityTool).IsAssignableFrom(i) && i != typeof(IUtilityTool)).ToList();

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