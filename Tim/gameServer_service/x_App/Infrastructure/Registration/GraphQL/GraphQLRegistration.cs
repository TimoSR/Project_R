using x_endpoints.ControllersGraphQL._Interface;

namespace x_endpoints.Infrastructure.Registration.GraphQL;

public static class GraphQlRegistration
{
    public static IServiceCollection AddGraphQLServices(this IServiceCollection services)
    {
        var queryTypes = GetTypes<BaseQuery>();
        var mutationTypes = GetTypes<BaseMutation>();
        var subscriptionTypes = GetTypes<BaseSubscription>();

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

    private static IEnumerable<Type> GetTypes<TBase>()
    {
        var baseType = typeof(TBase);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(baseType));
        return types;
    }
}
