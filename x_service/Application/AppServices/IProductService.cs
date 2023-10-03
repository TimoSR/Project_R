using Application.Registrations._Interfaces;
using Domain.DomainModels;

namespace Application.AppServices;

public interface IProductService : IAppService
{
    Task InsertAsync(Product data);
    Task<IEnumerable<Product>> GetAllAsync();
}