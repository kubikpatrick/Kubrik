using Kubrik.Api.Data;
using Kubrik.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Kubrik.Api.Hubs;

[Authorize]
public sealed class LocationHub : Hub
{
    private readonly ApplicationDbContext _context;
    
    private readonly IMemoryCache _cache;
    private readonly ILogger<LocationHub> _logger;
    
    public LocationHub(ApplicationDbContext context, IMemoryCache cache, ILogger<LocationHub> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }
    
    [HubMethodName(WsRegisteredEventNames.BroadcastLocation)]
    public async Task BroadcastLocationAsync(Location location)
    {
        _logger.LogInformation("Longitude: {longitude}, latitude: {latitude}", location.Longitude, location.Latitude);
    }
    
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (exception is not null)
        {
            _logger.LogError("{message}", exception.Message);
        }

        await base.OnDisconnectedAsync(exception);
    }
}