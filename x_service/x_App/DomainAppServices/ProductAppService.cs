using x_endpoints.DomainAppServices._Interface;
using x_endpoints.DomainEvents;
using x_endpoints.DomainRepositories._Interfaces;
using x_endpoints.Infrastructure.Helpers;
using x_endpoints.Infrastructure.Persistence._Interfaces;
using x_lib.DomainModels;

namespace x_endpoints.DomainAppServices;

public class ProductAppService : IAppService
{
    private readonly IEventManager _eventManager;
    private readonly ICacheManager _cacheManager;
    private readonly IRepository<Product> _productRepo;
    
    public ProductAppService(IServiceDependencies dependencies, IRepository<Product> productRepo)
    {
        _eventManager = dependencies.EventManager;
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
        
        await _eventManager.PublishJsonEventAsync(productCreatedEvent);
        await _eventManager.PublishProtobufEventAsync(productCreatedEvent);
    }

    // Query
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _productRepo.GetAllAsync();
    }
}