using Microsoft.Extensions.Caching.Memory;

namespace Ghb.Psicossoma.Cache
{
    public class CacheService
    {
        protected readonly IMemoryCache _memoryCache;
        protected readonly MemoryCacheEntryOptions _cacheEntryOptions;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

        }

        public string GetCacheEntry(string key, string value)
        {
            string resultText;

            if (!_memoryCache.TryGetValue(key, out string? cachedText))
            {
                resultText = value;
                _memoryCache.Set(key, value, _cacheEntryOptions);
            }
            else
            {
                resultText = cachedText!;
            }

            return resultText;
        }

        public object? GetGenericCacheEntry(string key, object value)
        {
            object? resultText;

            if (!_memoryCache.TryGetValue(key, out object? cachedText))
            {
                resultText = value;
                _memoryCache.Set(key, value, _cacheEntryOptions);
            }
            else
            {
                resultText = cachedText!;
            }

            return resultText;
        }

        public void SetGenericCacheEntry(string key, object? value)
        {
            _memoryCache.Set(key, value, _cacheEntryOptions);
        }
    }
}
