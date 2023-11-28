using Application.AppServices._Interfaces;
using Domain.DomainModels;
using Domain.IRepositories;

namespace Application.AppServices.V1;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    // Command
    public async Task InsertAsync(Product data)
    {
        await _productRepository.InsertAsync(data);
    }

    // Query
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _productRepository.GetAllAsync();
    }
}