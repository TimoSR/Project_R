using Application.Controllers.GraphQL.GraphQL._Interfaces;
using Domain.DomainModels;

namespace Application.Controllers.GraphQL.GraphQL.ProductCollection;

public class ProductSubscriptions : ISubscription
{
    [Subscribe]
    [Topic]
    public Product ProductAdded([EventMessage] Product product) => product;
}
