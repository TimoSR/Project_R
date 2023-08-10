using x_endpoints.Persistence.GraphQL_Server.Subscriptions;

namespace x_endpoints.ControllersGraphQL.Product;

public class ProductSubscriptions : BaseSubscription
{
    [Subscribe]
    [Topic]
    public Models.Product ProductAdded([EventMessage] Models.Product product) => product;
}
