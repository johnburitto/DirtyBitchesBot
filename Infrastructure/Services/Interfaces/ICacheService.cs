using System.Threading.Tasks;

namespace Infrastructure.Services.Interfaces
{
    public interface ICacheService
    {
        Task<T> SetValueAsync<T>(string key, T value);
        Task<T?> GetValueAsync<T>(string key);
        Task RemoveDataAsync(string key);
        Task<bool> IsDataCachedAsync(string key);
    }
}
