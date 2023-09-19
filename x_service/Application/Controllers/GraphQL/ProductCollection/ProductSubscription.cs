using Application.Registrations._Interfaces;
using Domain.DomainModels;

namespace Application.Controllers.GraphQL.ProductCollection;

public class ProductSubscriptions : ISubscription
{
    [Subscribe]
    [Topic]
    public Product ProductAdded([EventMessage] Product product) => product;
}
