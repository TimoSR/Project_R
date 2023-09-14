namespace x_App.Infrastructure.Registration.DataSeeder;

public interface IDataSeeder
{
    Task  SeedData(IServiceProvider serviceProvider);
}