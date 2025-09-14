using Kubrik.Models;
using Kubrik.Models.Devices;

namespace Kubrik.Services.Api;

public sealed class DeviceApiClient : BaseApiClient
{
    public DeviceApiClient(HttpClient http) : base(http)
    {
    }

    public async Task<Device[]> AllAsync()
    {
        return await SendAsync<Device[]>("/devices", HttpMethod.Get);
    }
    
    public async Task<Device> GetAsync(string id)
    {
        return await SendAsync<Device>($"/devices/{id}", HttpMethod.Get);
    }

    public async Task<Device> CreateAsync(string name, DeviceType type)
    {
        return await SendAsync<Device>($"/devices?name={name}&type={type}", HttpMethod.Post);
    }

    public async Task<Device> DeleteAsync(string id)
    {
        return await SendAsync<Device>($"/devices/{id}", HttpMethod.Delete);
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await SendAsync<bool>($"/devices/{id}/exists", HttpMethod.Get);
    }

    public async Task UpdateLocationAsync(string id, Location Location)
    {
        await SendAsync($"/devices/{id}/Location", HttpMethod.Patch, Location);
    }
}