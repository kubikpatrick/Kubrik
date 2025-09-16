using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Kubrik.Api.Data;
using Kubrik.Api.Hubs;
using Kubrik.Models;
using Kubrik.Models.Devices;
using Kubrik.Models.Results;

namespace Kubrik.Api.Services.Managers;

public sealed class DeviceManager : ManagerBase<Device>, IManager<Device>
{
    private readonly IHubContext<LocationHub> _hub;
    
    public DeviceManager(IHubContext<LocationHub> hub, ApplicationDbContext context, IMemoryCache cache) : base(context, cache)
    {
        _hub = hub;
    }

    public async Task<List<Device>> GetAllAsync(string userId)
    {
        var devices = await Context.Devices.Where(d => d.UserId == userId).ToListAsync();
        
        return devices;
    }

    public async Task<Device?> FindByIdAsync(string deviceId)
    {
        return await GetFromCacheOrFetchAsync(deviceId);
    }

    public async Task<DataResult> CreateAsync(Device device)
    {
        bool principal = await Context.Devices.AllAsync(d => !d.IsPrincipal);

        device.IsPrincipal = principal;
        
        await Context.Devices.AddAsync(device);
        await Context.SaveChangesAsync();

        Cache.Set(device.Id, device, new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(15)
        });
        
        return DataResult.Success();
    }

    public async Task DeleteAsync(Device device)
    {
        Context.Devices.Remove(device);
        Cache.Remove(device.Id);
        
        await Context.SaveChangesAsync();
    }
    
    public async Task UpdateLocationAsync(Device device, Location location)
    {
        await _hub.Clients.User(device.UserId).SendAsync(WsRegisteredEventNames.UpdateLocation, device.Id, location);

        device.Location = location;
        Context.Devices.Update(device);
        
        await Context.SaveChangesAsync();
    }
}