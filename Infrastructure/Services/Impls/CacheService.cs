using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services.Impls
{
    public class CacheService : ICacheService
    {
        private static CacheService? _instance;
        private MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public static CacheService Instance => GetInstance();

        private static CacheService GetInstance()
        {
            if (_instance == null)
            {
                lock(new object())
                {
                    _instance = new CacheService();
                }
            }

            return _instance;
        }

        public Task<T?> GetValueAsync<T>(string key)
        {
            return Task.FromResult(_cache.Get<T>(key));
        }

        public Task<bool> IsDataCachedAsync(string key)
        {
            return Task.FromResult(_cache.TryGetValue(key, out var _));
        }

        public Task RemoveDataAsync(string key)
        {
            _cache.Remove(key);

            return Task.CompletedTask;
        }

        public Task<T> SetValueAsync<T>(string key, T value)
        {
            return Task.FromResult(_cache.Set(key, value));
        }
    }
}
