using System;
using System.Linq;
using System.Reflection;
using CommonLibrary.Infrastructure.Utilities._Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CommonLibrary.Infrastructure.Registrations.Utilities
{
    public static class UtilityRegistration
    {
        public static IServiceCollection RegisterUtilityServices(this IServiceCollection services)
        {
            // Get all loaded assemblies
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Optionally, filter assemblies here based on some criteria, e.g., name, custom attributes, etc.

            foreach (var assembly in assemblies)
            {
                // Fetch all types that are classes, not abstract, and implement the IUtilityTool interface.
                var types = assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Contains(typeof(IUtilityTool)));

                foreach (var type in types)
                {
                    // For each type, find the interface that matches the IUtilityTool.
                    // This assumes that the class only implements one interface that is IUtilityTool.
                    // If there are multiple, you might need more complex logic to find the right one.
                    var serviceType = type.GetInterfaces().FirstOrDefault(i => typeof(IUtilityTool).IsAssignableFrom(i));
                    if (serviceType != null)
                    {
                        // Register the type with the DI container as a singleton.
                        services.AddSingleton(serviceType, type);
                    }
                }
            }

            return services;
        }
    }
}