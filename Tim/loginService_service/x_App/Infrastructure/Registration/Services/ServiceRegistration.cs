using System.Reflection;
using x_endpoints.DomainAppServices;
using x_endpoints.DomainAppServices._Interface;

namespace x_endpoints.Infrastructure.Registration.Services;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {   
        
        // Using reflection to get all types which are classes, not abstract, and implement IService
        var serviceTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(IAppService).IsAssignableFrom(t));

        foreach (var type in serviceTypes)
        {
            services.AddTransient(type);
        }

        return services;
    }
}