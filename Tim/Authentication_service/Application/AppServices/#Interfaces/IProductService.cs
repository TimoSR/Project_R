using _SharedKernel.Patterns.RegistrationHooks.Services._Interfaces;
using Domain.DomainModels;

namespace Application.AppServices._Interfaces;

public interface IProductService : IAppService
{
    Task InsertAsync(Product data);
    Task<IEnumerable<Product>> GetAllAsync();
}