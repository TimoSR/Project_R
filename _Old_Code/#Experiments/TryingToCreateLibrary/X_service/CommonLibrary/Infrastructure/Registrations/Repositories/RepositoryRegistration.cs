using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using CommonLibrary.Infrastructure.Persistence._Interfaces;

namespace CommonLibrary.Infrastructure.Registrations.Repositories
{
    public static class RepositoryRegistration
    {
        public static IServiceCollection AddApplicationRepositories(this IServiceCollection services, Assembly[] additionalAssemblies = null)
        {
            // Get all loaded assemblies and any additional specified assemblies
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            if (additionalAssemblies != null)
            {
                assemblies.AddRange(additionalAssemblies);
            }
            
            // Optionally filter assemblies here based on some criteria, e.g., name, custom attributes, etc.
            // assemblies = assemblies.Where(a => a.FullName.StartsWith("YourProjectPrefix")).ToList();

            foreach (var assembly in assemblies)
            {
                // Get the repository types from the assembly
                var types = assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                        .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<>)))
                    .ToList();

                // Register each type with its corresponding IRepository<> interface
                foreach (var type in types)
                {
                    var serviceType = type.GetInterfaces().FirstOrDefault(i => i.IsGenericType &&
                                                                               i.GetGenericTypeDefinition() == typeof(IRepository<>));
                    if (serviceType != null)
                    {
                        services.AddScoped(serviceType, type);
                    }
                }
            }

            return services;
        }
    }
}