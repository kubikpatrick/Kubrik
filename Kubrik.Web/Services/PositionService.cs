using Kubrik.Models;
using Kubrik.Services;
using Kubrik.Services.Contracts;
using Microsoft.JSInterop;

namespace Kubrik.Web.Services;

public sealed class LocationService : ILocationService
{
    private readonly IGeolocationService _geolocation;
    
    public LocationService(IGeolocationService geolocation)
    {
        _geolocation = geolocation;
    }

    public async Task<Location?> GetCurrentLocationAsync()
    {
        throw new NotImplementedException();
    }
}