using x_endpoints.Models;

namespace x_endpoints.GraphQL.Subscriptions;

public class ProductSubscriptions : BaseSubscription
{
    [Subscribe]
    [Topic]
    public Product ProductAdded([EventMessage] Product product) => product;
}
