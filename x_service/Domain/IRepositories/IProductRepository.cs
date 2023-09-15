using x_Domain.DomainModels;

namespace x_Domain.IRepositories;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetProductsByNameAsync(string productName);
}