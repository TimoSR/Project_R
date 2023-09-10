using System.Reflection;
using x_endpoints.DomainServices;
using x_endpoints.Persistence.MongoDB;

namespace x_endpoints.Registration.Services;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {   
        
        // Using reflection to get all types which are classes, not abstract, and inherit from BaseService<T>
        var serviceTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(BaseService<>));

        foreach (var type in serviceTypes)
        {
            services.AddTransient(type);
        }

        return services;
        
        // // Using reflection to get all types which are classes, not abstract, and inherit from BaseService
        // var serviceTypes = Assembly.GetExecutingAssembly().GetTypes()
        //     .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(BaseService)));
        //
        // foreach (var type in serviceTypes)
        // {
        //     services.AddTransient(type);
        // }
        //
        // return services;
    }
}