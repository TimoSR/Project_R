using x_App.Controllers.GraphQL._Interface;
using x_Domain.DomainModels;

namespace x_App.Controllers.GraphQL.ProductCollection;

public class ProductSubscriptions : ISubscription
{
    [Subscribe]
    [Topic]
    public Product ProductAdded([EventMessage] Product product) => product;
}
