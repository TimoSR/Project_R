using Domain.DomainModels;

namespace Domain.IRepositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsByNameAsync(string productName);
}