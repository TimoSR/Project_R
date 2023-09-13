using x_endpoints.ControllersGraphQL._Interface;

namespace x_endpoints.Controllers.GraphQL.ProductCollection;

public class ProductSubscriptions : BaseSubscription
{
    [Subscribe]
    [Topic]
    public x_lib.DomainModels.Product ProductAdded([EventMessage] x_lib.DomainModels.Product product) => product;
}
