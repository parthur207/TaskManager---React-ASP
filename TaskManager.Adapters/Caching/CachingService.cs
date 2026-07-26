using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TaskManager.Core.Ports.Caching;

namespace TaskManager.Adapters.Caching;

public class CachingService : ICachingPort
{
    private readonly IDistributedCache _cache;

    public CachingService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _cache.GetStringAsync(key);
        if (string.IsNullOrEmpty(value))
            return default;

        return JsonSerializer.Deserialize<T>(value);
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiration = null)
    {
        var options = new DistributedCacheEntryOptions();

        if (expiration.HasValue)
            options.AbsoluteExpirationRelativeToNow = expiration;

        var json = JsonSerializer.Serialize(value);

        await _cache.SetStringAsync(
            key,
            json,
            options);
    }

    public async Task RemoveAsync(string key)
    {
        if(_cache != null)
            await _cache.RemoveAsync(key);
    }
}