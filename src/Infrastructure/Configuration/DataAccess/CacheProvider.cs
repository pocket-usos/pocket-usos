using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace App.Infrastructure.Configuration.DataAccess;

internal class CacheProvider(IDistributedCache cache) : ICacheProvider
{
    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        var value = await cache.GetStringAsync(key);

        return value == null ? null : JsonSerializer.Deserialize<T>(value);
    }

    public async Task SetAsync<T>(string key, T value, Action<DistributedCacheEntryOptions>? setOptions = null) where T : class
    {
        var options = new DistributedCacheEntryOptions();
        setOptions?.Invoke(options);

        var serializedValue = JsonSerializer.Serialize(value);

        await cache.SetStringAsync(key, serializedValue , options);
    }

    public async Task RemoveAsync(string key)
    {
        await cache.RemoveAsync(key);
    }
}
