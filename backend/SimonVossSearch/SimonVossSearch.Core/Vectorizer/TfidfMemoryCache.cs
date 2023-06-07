using Microsoft.Extensions.Caching.Memory;

namespace SimonVossSearch.Core.Vectorizer;

public static class TfidfMemoryCache
{
    private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    public static TfidfVectorizer GetOrCreate(string key)
    {
        TfidfVectorizer cacheEntry;
        if (!_cache.TryGetValue(key, out cacheEntry))
        {
            cacheEntry = new TfidfVectorizer();
            
            _cache.Set(key, cacheEntry);
        }
        return cacheEntry;
    }
}