using System.Threading.Tasks;

namespace Genesis.Common
{
    public interface ICachingService
    {
        CacheItem IsAvailable(string key);

        T LoadCachedData<T>(string key) where T : class;

        void StoreCachedData<T>(string key, T dataToCache) where T : class;

        void Clear(string cacheKey);
    }
}