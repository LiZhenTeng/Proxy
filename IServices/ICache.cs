using Microsoft.Extensions.Caching.Memory;

namespace Proxy.IServices;

public interface ICache
{
    public object? Get(object key);
    public bool TryGetValue<TItem>(object key, out TItem? value);
    public TItem? GetOrCreate<TItem>(object key, Func<ICacheEntry, TItem> factory);
    public Task<TItem?> GetOrCreateAsync<TItem>(object key, Func<ICacheEntry, Task<TItem>> factory);
    public TItem? Set<TItem>(object key, TItem value);
}