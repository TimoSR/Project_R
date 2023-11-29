using CommonLibrary.Application.Registrations._Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CommonLibrary.Application.Registrations.GraphQL;

public static class GraphQlRegistration
{
    public static IServiceCollection AddGraphQlServices(this IServiceCollection services)
    {
        var queryTypes = GetTypes<IQuery>();
        var mutationTypes = GetTypes<IMutation>();
        var subscriptionTypes = GetTypes<ISubscription>();

        var server = services.AddGraphQLServer();

        // Dynamically register queries
        foreach (var type in queryTypes)
        {
            server.AddQueryType(type);
        }

        // Dynamically register mutations
        foreach (var type in mutationTypes)
        {
            server.AddMutationType(type);
        }

        // Dynamically register subscriptions
        foreach (var type in subscriptionTypes)
        {
            server.AddSubscriptionType(type);
        }

        return services;
    }

    private static IEnumerable<Type> GetTypes<TInterface>() where TInterface : class
    {
        var interfaceType = typeof(TInterface);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsClass && interfaceType.IsAssignableFrom(type));
        return types;
    }
}
