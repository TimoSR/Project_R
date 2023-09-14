using x_App.Controllers.GraphQL._Interface;

namespace x_App.Controllers.GraphQL.ProductCollection;

public class ProductSubscriptions : ISubscription
{
    [Subscribe]
    [Topic]
    public x_lib.DomainModels.Product ProductAdded([EventMessage] x_lib.DomainModels.Product product) => product;
}
