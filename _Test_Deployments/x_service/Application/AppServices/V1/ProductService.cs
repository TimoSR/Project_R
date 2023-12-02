using Application.AppServices.V1._Interfaces;
using Domain.DomainEvents;
using Domain.DomainModels;
using Domain.IRepositories;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.Utilities.Containers;

namespace Application.AppServices.V1;

public class ProductService : IProductService
{
    private readonly IEventPublisher _eventPublisher;
    private readonly ICacheManager _cacheManager;
    private readonly IProductRepository _productRepository;
    
    public ProductService(IServiceDependencies dependencies, IProductRepository productRepository)
    {
        _eventPublisher = dependencies.EventPublisher;
        _cacheManager = dependencies.CacheManager;
        _productRepository = productRepository;
    }

    // Command
    public async Task InsertAsync(Product data)
    {
        await _productRepository.InsertAsync(data);
        
        var productCreatedEvent = new ProductCreatedEvent
        {
            test = "hello"
        };
        
        await _eventPublisher.PublishJsonEventAsync(productCreatedEvent);
        await _eventPublisher.PublishProtobufEventAsync(productCreatedEvent);
    }

    // Query
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _productRepository.GetAllAsync();
    }
}