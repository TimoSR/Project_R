namespace Infrastructure.Persistence._Interfaces;

public interface IRepository<T>
{
    Task InsertAsync(T data);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(string id);
    Task UpdateAsync(string id, T updatedData);
    Task<bool> DeleteAsync(string id);
}