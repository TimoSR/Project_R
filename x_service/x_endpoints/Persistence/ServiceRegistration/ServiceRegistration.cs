using System.Reflection;

namespace x_endpoints.Persistence.ServiceRegistration;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Using reflection to get all types which are classes, not abstract, and inherit from BaseService
        var serviceTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(BaseService)));

        foreach (var type in serviceTypes)
        {
            services.AddTransient(type);
        }

        return services;
    }
}