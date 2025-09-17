using Kubrik.Api.Data;
using Kubrik.Api.Miscellaneous.Results;
using Kubrik.Models.Circles;

using Microsoft.EntityFrameworkCore;
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
        
        Cache.Set(circle.Id, circle, new MemoryCacheEntryOptions
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

    public async Task<DataResult> AddMemberAsync(Circle circle, Member member)
    {
        bool exists = await Context.Circles.AnyAsync(c => c.Id == circle.Id && c.Members.Any(m => m.UserId == member.UserId));
        if (exists)
        {
            return DataResult.Fail("User already exists in this circle.", StatusCodes.Status409Conflict);
        }

        await Context.Members.AddAsync(member);
        await Context.SaveChangesAsync();
        
        return DataResult.Success();
    }

    public async Task<DataResult> RemoveMemberAsync(Circle circle, string userId)
    {
        var member = await Context.Members.FirstOrDefaultAsync(m => m.UserId == userId && m.CircleId == circle.Id);
        if (member is null)
        {
            return DataResult.Fail("User is not a member of this circle.", StatusCodes.Status404NotFound);
        }
        
        Context.Members.Remove(member);
        await Context.SaveChangesAsync();
        
        return DataResult.Success();   
    }
}