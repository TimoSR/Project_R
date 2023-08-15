using x_endpoints.Services;

namespace x_endpoints;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Add your MongoDB services here.
        services.AddTransient<ProductService>();
        services.AddTransient<OreService>();
        services.AddTransient<CharacterService>();

        // Add more services as needed.

        return services;
    }
}