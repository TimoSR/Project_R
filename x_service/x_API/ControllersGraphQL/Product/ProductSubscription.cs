using x_endpoints.Registration.GraphQL.Subscriptions;

namespace x_endpoints.ControllersGraphQL.Product;

public class ProductSubscriptions : BaseSubscription
{
    [Subscribe]
    [Topic]
    public x_lib.DomainModels.Product ProductAdded([EventMessage] x_lib.DomainModels.Product product) => product;
}
