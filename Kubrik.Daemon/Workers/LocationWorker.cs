using System.Net.Http.Json;

using Kubrik.Models;
using Kubrik.Daemon.Services;

using Windows.Devices.Geolocation;

namespace Kubrik.Daemon.Workers;

public sealed class LocationWorker : BackgroundService
{
    private readonly HttpClient _http;
    private readonly SemaphoreSlim _lock;
    private readonly DeviceIdService _deviceIdService;
    
    private readonly ILogger<LocationWorker> _logger;

    public LocationWorker(HttpClient http, SemaphoreSlim @lock, DeviceIdService deviceIdService, ILogger<LocationWorker> logger)
    {
        _http = http;
        _lock = @lock;
        _deviceIdService = deviceIdService;
        _logger = logger;
    }
    
    private Geolocator _geolocator;

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);
        
        var access = await Geolocator.RequestAccessAsync();
        if (access != GeolocationAccessStatus.Allowed)
        {
            _logger.LogError("Access denied.");
            
            return;
        }
        
        _geolocator = new Geolocator
        {
            DesiredAccuracy = PositionAccuracy.High,
        };

        _geolocator.PositionChanged += LocationChanged;

        _lock.Release();
        
        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _geolocator.PositionChanged -= LocationChanged;
        
        await base.StopAsync(cancellationToken);
    }

    private async void LocationChanged(object sender, PositionChangedEventArgs e)
    {
        double longitude = e.Position.Coordinate.Longitude;
        double latitude = e.Position.Coordinate.Latitude;
        
        try
        {
            string id = await _deviceIdService.GetIdOrCreateAsync();
            
            var response = await _http.PatchAsJsonAsync($"/devices/{id}/location", new Location
            {
                Longitude = longitude,
                Latitude = latitude,
                Timestamp = DateTime.UtcNow,
                Type = LocationType.Device
            });

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error: {message}", await response.Content.ReadAsStringAsync());
            }
            else
            {
                _logger.LogInformation("Location updated successfully at {time}", DateTime.UtcNow.TimeOfDay);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error: {message}", ex.Message);
        }
    }
}