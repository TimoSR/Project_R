namespace Infrastructure.Persistence._Interfaces;

public interface ICacheManager
{
    Task SetValue(string key, string value);
    Task GetValue(string key);
}