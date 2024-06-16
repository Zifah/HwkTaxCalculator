using Core.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryCacheService(IMemoryCache memoryCache) 
        {
            _memoryCache = memoryCache;
        }

        public void CacheItem<T>(string key, T value)
        {
            _memoryCache.Set(key, value);
        }

        public T? RetrieveItem<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }
    }
}
