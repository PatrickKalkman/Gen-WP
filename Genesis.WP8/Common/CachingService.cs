using System;
using System.IO;

using Windows.Storage;

using Genesis.Common;

using Newtonsoft.Json;

namespace Genesis.WP8.Common
{
    public class CachingService : ICachingService
    {
        private const string CacheFileNameFormat = "Cache_{0}_Data.json";

        public CacheItem IsAvailable(string key)
        {
            return ContainsFileAsync(GenerateCacheFileName(key));
        }

        private CacheItem ContainsFileAsync(string filename)
        {
            try
            {
                StorageFile file = ApplicationData.Current.LocalFolder.GetFileAsync(filename).GetAwaiter().GetResult();
                return new CacheItem
                {
                    IsAvailable = true,
                };
            }
            catch (FileNotFoundException ex)
            {
                return new CacheItem
                {
                    IsAvailable = false
                };
            }
        }

        public T LoadCachedData<T>(string key) where T : class
        {
            try
            {
                CacheItem cacheItem = IsAvailable(key);
                if (cacheItem.IsAvailable)
                {
                    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                    StorageFile cacheStorageFile = localFolder.GetFileAsync(GenerateCacheFileName(key)).GetResults();
                    using (Stream cacheStream = cacheStorageFile.OpenStreamForReadAsync().Result)
                    {
                        using (var reader = new StreamReader(cacheStream))
                        {
                            string cacheDataString = reader.ReadToEndAsync().Result;
                            T cacheData = JsonConvert.DeserializeObject<T>(cacheDataString);
                            return cacheData;
                        }
                    }
                }
                return null;
            }
            catch (Exception error)
            {
                return null;
            }
        }

        public async void StoreCachedData<T>(string key, T dataToCache) where T : class
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile cacheStorageFile = await localFolder.CreateFileAsync(GenerateCacheFileName(key), CreationCollisionOption.ReplaceExisting);
            using (Stream cacheStream = await cacheStorageFile.OpenStreamForWriteAsync())
            {
                using (var writer = new StreamWriter(cacheStream))
                {
                    string cacheDataString = JsonConvert.SerializeObject(dataToCache);
                    await writer.WriteAsync(cacheDataString);
                }
            }
        }

        private static string GenerateCacheFileName(string key)
        {
            return string.Format(CacheFileNameFormat, key);
        }

        public void Clear(string cacheKey)
        {
            CacheItem cacheItem = ContainsFileAsync(GenerateCacheFileName(cacheKey));
            if (cacheItem.IsAvailable)
            {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFile cacheStorageFile = localFolder.GetFileAsync(GenerateCacheFileName(cacheKey)).GetResults();
                cacheStorageFile.DeleteAsync().GetResults();
            }
        }
    }
}
