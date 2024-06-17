namespace Core.Services
{
    public interface ICacheService
    {
        void CacheItem<T>(string key, T value);
        T? RetrieveItem<T>(string key);
    }
}
