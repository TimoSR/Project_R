using ServiceLibrary.Persistence._Interfaces;
using x_App.Infrastructure.Containers;
using x_App.Infrastructure.Reflectors._Interfaces;
using x_Domain.DomainEvents;
using x_Domain.DomainModels;

namespace x_App.DomainAppServices;

public class ProductAppService : IAppService
{
    private readonly IEventPublisher _eventPublisher;
    private readonly ICacheManager _cacheManager;
    private readonly IRepository<Product> _productRepo;
    
    public ProductAppService(IServiceDependencies dependencies, IRepository<Product> productRepo)
    {
        _eventPublisher = dependencies.EventPublisher;
        _cacheManager = dependencies.CacheManager;
        _productRepo = productRepo;
    }

    // Command
    public async Task InsertAsync(Product data)
    {
        await _productRepo.InsertAsync(data);
        
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
        return await _productRepo.GetAllAsync();
    }
}