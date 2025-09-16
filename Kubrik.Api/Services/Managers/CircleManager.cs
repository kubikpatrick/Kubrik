using Kubrik.Api.Data;
using Kubrik.Models.Circles;
using Kubrik.Models.Results;

using Microsoft.Extensions.Caching.Memory;

namespace Kubrik.Api.Services.Managers;

public sealed class CircleManager : ManagerBase<Circle>, IManager<Circle>
{
    public CircleManager(ApplicationDbContext context, IMemoryCache cache) : base(context, cache)
    {
    }

    public async Task<Circle?> FindByIdAsync(string circleId)
    {
        return await GetFromCacheOrFetchAsync(circleId);
    }

    public async Task<DataResult> CreateAsync(Circle circle)
    {
        await Context.Circles.AddAsync(circle);
        await Context.SaveChangesAsync();
        
        Cache.Set(circle.Id,  circle, new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(15)
        });
        
        return DataResult.Success();
    }

    public async Task DeleteAsync(Circle circle)
    {
        Context.Circles.Remove(circle);
        Cache.Remove(circle.Id);
        
        await Context.SaveChangesAsync();
    }
}