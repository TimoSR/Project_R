using Domain.Common.DomainModels;

namespace Domain.Common.IRepositories;

public interface IProductRepository
{
    Task<Product> GetProductsByNameAsync(string name);
    Task InsertAsync(Product data);
    Task<IEnumerable<Product>> GetAllAsync();
}