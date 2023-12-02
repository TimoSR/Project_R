using System.Reflection;
using _SharedKernel.Patterns.RegistrationHooks.Services._Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Domain._Shared.Registration;

public static class DomainServiceRegistration
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        // Fetch all types that are classes, not abstract, and implement the IDomainService interface.
        var types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(IDomainService).IsAssignableFrom(t));

        foreach (var type in types)
        {
            // Register the type directly without using its interface.
            services.AddScoped(type);
        }

        return services;
    }
}