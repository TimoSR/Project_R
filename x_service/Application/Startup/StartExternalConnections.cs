using Application.Registrations.Events;
using Infrastructure.Persistence._Interfaces;
using ServiceLibrary.Persistence._Interfaces;

namespace Application.Startup;

public class StartExternalConnections : IHostedService
{
    private readonly IMongoDbManager _mongoDbManager;
    private readonly PubTopicsRegister _pubTopicsRegister;
    private readonly SubTopicsRegister _subTopicsRegister;
    private readonly ICacheManager _cacheManager;

    public StartExternalConnections(IServiceProvider serviceProvider, IMongoDbManager mongoDbManager, PubTopicsRegister pubTopicsRegister, SubTopicsRegister subTopicsRegister, ICacheManager cacheManager)
    {
        // This makes the MongoDB Optional
        _mongoDbManager = serviceProvider.GetService<IMongoDbManager>();
        _pubTopicsRegister = serviceProvider.GetService<PubTopicsRegister>();
        _subTopicsRegister = serviceProvider.GetService<SubTopicsRegister>();
        _cacheManager = serviceProvider.GetService<ICacheManager>();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Since the work is done, return a completed Task
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // There is nothing to do on stop, so just return a completed Task
        return Task.CompletedTask;
    }
}