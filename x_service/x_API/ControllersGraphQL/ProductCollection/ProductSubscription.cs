using x_endpoints.Infrastructure.Registration.GraphQL.Subscriptions;

namespace x_endpoints.ControllersGraphQL.ProductCollection;

public class ProductSubscriptions : BaseSubscription
{
    [Subscribe]
    [Topic]
    public x_lib.DomainModels.Product ProductAdded([EventMessage] x_lib.DomainModels.Product product) => product;
}
