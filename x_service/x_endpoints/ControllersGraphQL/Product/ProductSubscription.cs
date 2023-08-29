using x_endpoints.Registration.GraphQL.Subscriptions;

namespace x_endpoints.ControllersGraphQL.Product;

public class ProductSubscriptions : BaseSubscription
{
    [Subscribe]
    [Topic]
    public DomainModels.Product ProductAdded([EventMessage] DomainModels.Product product) => product;
}
