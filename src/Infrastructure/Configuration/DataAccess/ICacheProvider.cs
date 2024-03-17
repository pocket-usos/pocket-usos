using Microsoft.Extensions.Caching.Distributed;

namespace App.Infrastructure.Configuration.DataAccess;

public interface ICacheProvider
{
    public Task<T?> GetAsync<T>(string key) where T : class;

    public Task SetAsync<T>(string key, T value, Action<DistributedCacheEntryOptions>? setOptions = null) where T : class;

    public Task RemoveAsync(string key);
}
