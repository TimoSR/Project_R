using x_endpoints.GraphQL.Mutations;
using x_endpoints.GraphQL.Queries;
using x_endpoints.GraphQL.Subscriptions;

namespace x_endpoints.GraphQL;

public static class GraphQLRegistration
{
    public static IServiceCollection AddGraphQLServices(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .AddQueryType<Query>()
            .AddMutationType<Mutation>()
            .AddSubscriptionType<Subscription>()
            .AddInMemorySubscriptions();

        return services;
    }
}