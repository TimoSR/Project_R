using x_App.Infrastructure.Reflectors._Interfaces;
using x_Domain.DomainModels;

namespace x_App.Controllers.GraphQL.ProductCollection;

public class ProductSubscriptions : ISubscription
{
    [Subscribe]
    [Topic]
    public Product ProductAdded([EventMessage] Product product) => product;
}
