using Domain.DomainModels;

namespace Domain.IRepositories;

public interface IProductRepository
{
    Task<Product> GetProductsByNameAsync(string name);
    Task InsertAsync(Product data);
    Task<IEnumerable<Product>> GetAllAsync();
}