using Microsoft.Extensions.Caching.Memory;
using SimonVossSearch.Core.Parser;

namespace SimonVossSearch.Core.Vectorizer;

public static class TfidfMemoryCache
{
    private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    public static TfidfVectorizer GetOrCreate(string key, IDataFileParser parser)
    {
        TfidfVectorizer cacheEntry;
        if (!_cache.TryGetValue(key, out cacheEntry))
        {
            cacheEntry = new TfidfVectorizer(parser);
            
            _cache.Set(key, cacheEntry);
        }
        return cacheEntry;
    }
}