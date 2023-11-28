using _SharedKernel.Patterns.ResultPattern;
using Application.Controllers.GraphQL.GraphQL._Interfaces;

namespace Application.Controllers.GraphQL.GraphQL.ProductCollection;

public class UserSubscriptions : ISubscription
{
    [Subscribe]
    [Topic]
    public ServiceResult UserAdded([EventMessage] ServiceResult registeredUser) => registeredUser;
}
