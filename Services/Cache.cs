using Microsoft.Extensions.Caching.Memory;
using Proxy.IServices;

namespace Proxy.Services;

public class Cache : ICache
{
    private readonly IMemoryCache _memoryCache;
    private readonly IConfiguration _configuration;
    private readonly ILogger<Cache> _logger;
    public Cache(IMemoryCache memoryCache, IConfiguration configuration, ILogger<Cache> logger)
    {
        _memoryCache = memoryCache;
        _configuration = configuration;
        _logger = logger;
    }
    public object? Get(object key)
    {
        try
        {
            return _memoryCache.Get(key);
        }
        catch (System.Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
    public bool TryGetValue<TItem>(object key, out TItem? v)
    {
        var b = _memoryCache.TryGetValue<TItem>(key, out TItem? value);
        v = value;
        return b;
    }

    public TItem? GetOrCreate<TItem>(object key, Func<ICacheEntry, TItem> factory)
    {
        try
        {
            return _memoryCache.GetOrCreate(key, (e) =>
            {
                var absoluteExpirationTime = _configuration.GetValue<double>("MemoryCache:AbsoluteExpirationTime");
                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(absoluteExpirationTime);
                return factory(e);
            });

        }
        catch (System.Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
    public Task<TItem?> GetOrCreateAsync<TItem>(object key, Func<ICacheEntry, Task<TItem>> factory)
    {
        try
        {
            return _memoryCache.GetOrCreateAsync(key, (e) =>
            {
                var absoluteExpirationTime = _configuration.GetValue<double>("MemoryCache:AbsoluteExpirationTime");
                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(absoluteExpirationTime);
                return factory(e);
            });

        }
        catch (System.Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }

    public TItem? Set<TItem>(object key, TItem value)
    {
        try
        {
            var absoluteExpirationTime = _configuration.GetValue<double>("MemoryCache:AbsoluteExpirationTime");
            return _memoryCache.Set(key, value, TimeSpan.FromSeconds(absoluteExpirationTime));
        }
        catch (System.Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}