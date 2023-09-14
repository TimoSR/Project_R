namespace x_App.Infrastructure.Reflectors.DataSeeder;

public interface IDataSeeder
{
    Task  SeedData(IServiceProvider serviceProvider);
}