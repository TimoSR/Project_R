using Application.Registrations._Interfaces;
using Domain.Common.DomainModels;
using Domain.DomainModels;

namespace Application.AppServices.V1._Interfaces;

public interface IProductService : IAppService
{
    Task InsertAsync(Product data);
    Task<IEnumerable<Product>> GetAllAsync();
}