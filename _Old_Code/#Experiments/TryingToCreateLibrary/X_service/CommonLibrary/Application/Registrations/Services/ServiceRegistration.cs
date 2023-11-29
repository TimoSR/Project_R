using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using CommonLibrary.Application.Registrations._Interfaces;

namespace CommonLibrary.Application.Registrations.Services
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {   
            // Fetch all types that are classes and implement the IAppService interface from the calling assembly.
            var types = Assembly.GetCallingAssembly().GetTypes().Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                t.GetInterfaces().Contains(typeof(IAppService))
            );

            foreach (var type in types)
            {
                // For each type, find the interfaces that extend IAppService
                // and register them with the DI container.
                var serviceInterfaces = type.GetInterfaces()
                    .Where(i => typeof(IAppService).IsAssignableFrom(i) && i != typeof(IAppService));

                foreach (var serviceInterface in serviceInterfaces)
                {
                    // Register the type with the DI container.
                    services.AddScoped(serviceInterface, type);

                    // Output the registered service to the console.
                    Console.WriteLine($"Registered service: {serviceInterface.FullName} with implementation: {type.FullName}");
                }
            }

            return services;
        }
    }
}