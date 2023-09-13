namespace x_endpoints.Infrastructure.Registration.DataSeeder;

public interface IDataSeeder
{
    Task  SeedData(IServiceProvider serviceProvider);
}