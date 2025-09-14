using Microsoft.Extensions.Caching.Memory;

using Kubrik.Api.Data;

namespace Kubrik.Api.Services.Managers;

public abstract class ManagerBase<T> where T : class
{
    protected readonly ApplicationDbContext Context;
    
    protected readonly IMemoryCache Cache;

    protected ManagerBase(ApplicationDbContext context, IMemoryCache cache)
    {
        Context = context;
        Cache = cache;
    }

    private protected async Task<T?> GetFromCacheOrFetchAsync(string id)
    {
        if (Cache.TryGetValue(id, out T? value))
        {
            return value;
        }

        value = await Context.FindAsync<T>(id);
        if (value is not null)
        {
            Cache.Set(id, value, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(15)
            });
        }
        
        return value;
    }
}