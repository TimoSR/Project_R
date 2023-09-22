using System.Reflection;
using Infrastructure.Persistence._Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Registrations.Repositories;

public static class RepositoryRegistration
{
    public static IServiceCollection AddApplicationRepositories(this IServiceCollection services)
    {
        
        // Using reflection to get all types which are classes, not abstract, and implement IRepository<T>
        var repoTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.BaseType != null && t.BaseType.IsGenericType && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<>)))
            .ToList();

        foreach (var type in repoTypes)
        {
            // Extract the type T from the base class (e.g., from MongoRepository<T>)
            var entityType = type.BaseType.GetGenericArguments()[0];

            // Construct the IRepository<T> type for the found entity type
            var repoInterfaceType = typeof(IRepository<>).MakeGenericType(entityType);

            services.AddScoped(type);
        }

        return services;
    }
}