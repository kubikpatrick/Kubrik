using System.Net.Http.Json;

using Kubrik.Models;
using Kubrik.Models.Circles;
using Kubrik.Models.Devices;
using Kubrik.Services.Contracts;
using Kubrik.Web.Services;

using Community.Blazor.MapLibre;
using Community.Blazor.MapLibre.Models;
using Community.Blazor.MapLibre.Models.Control;
using Community.Blazor.MapLibre.Models.Marker;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Kubrik.Web.Pages;

[Authorize]
public partial class Home : ComponentBase
{
    private readonly HttpClient _http;

    private readonly IServerUrlService _serverUrlService;
    private readonly ITokenService _tokenService;
    
    public Home(ITokenService tokenService, IServerUrlService serverUrlService, HttpClient http)
    {
        _http = http;
        _tokenService = tokenService;
        _serverUrlService = serverUrlService;
    }

    private MarkerService _markerService => new MarkerService(_map);
    private MapLibre _map = new MapLibre();
    private MapOptions _options = new MapOptions
    {
        MaxZoom = 23,
        MinZoom = 2,
        Zoom = 2,
        Style = "style.json",
        CanvasContextAttributes = new WebGLContextAttributes
        {
            Antialias = true,
            ContextType = "webgl2"
        }
    };

    public List<Circle> Circles { get; set; } = [];
    public List<Device> Devices { get; set; } = [];

    private HubConnection _connection;

    protected override async Task OnInitializedAsync()
    {
        string? url = await _serverUrlService.GetServerUrlAsync();
 
        _connection = new HubConnectionBuilder()
            .WithUrl($"{url}/hubs/location", options =>
            {
                options.AccessTokenProvider = async () => await _tokenService.GetTokenAsync();
            })
            .WithAutomaticReconnect()
            .Build();
        
        _connection.On<string, Location>(WsRegisteredEventNames.UpdateLocation, async (id, location) =>
        {
            await _markerService.MoveMarkerAsync(id, location);
        });

        Circles = await _http.GetFromJsonAsync<List<Circle>>($"{url}/circles");
        Devices = await _http.GetFromJsonAsync<List<Device>>($"{url}/devices");
    }
    
    private async Task OnLoad()
    {
        foreach (var device in Devices)
        {
            await _markerService.AddMarkerAsync(device.Id, device.Location, new MarkerOptions
            {
                OpacityWhenCovered = "0",
                Extensions = new MarkerOptionsExtensions
                {
                    HtmlContent = 
                    $"""
                        <div class="d-flex justify-content-center align-items-center rounded-circle bg-white shadow" style="width: 50px; height: 50px; border: 2px solid #dee2e6;">
                            <img src="/images/{(device.Type is DeviceType.Computer ? "computer.png" : "smartphone.png")}" class="rounded-circle" style="width: 32px; height: 32px;">
                        </div>
                    """
                }
            });
        }
        
        await _map.AddControl(ControlType.NavigationControl, ControlPosition.TopRight);
        await _map.AddControl(ControlType.FullscreenControl, ControlPosition.TopRight);
        await _map.AddControl(ControlType.GlobeControl, ControlPosition.TopRight);
        
        await _connection.StartAsync();
    }
}