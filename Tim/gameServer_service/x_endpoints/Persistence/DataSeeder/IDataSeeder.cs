namespace x_endpoints.Persistence.DataSeeder;

public interface IDataSeeder
{
    Task  SeedData(IServiceProvider serviceProvider);
}