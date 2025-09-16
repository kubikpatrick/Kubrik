using System.Net.Http.Json;

using Kubrik.Models.Devices;

namespace Kubrik.Daemon.Services;

public sealed class DeviceIdService
{
    private readonly HttpClient _http;
    
    public DeviceIdService(HttpClient http)
    {
        _http = http;
    }

    public async Task<string> GetIdOrCreateAsync()
    {
        string path = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Kubrik.txt");
        if (File.Exists(path))
        {
            return await File.ReadAllTextAsync(path);
        }
        
        string id = await CreateAsync(Environment.MachineName, DeviceType.Computer);
        
        await File.WriteAllTextAsync(path, id);

        return id;
    }

    private async Task<string> CreateAsync(string machineName, DeviceType type)
    {
        var response = await _http.PostAsJsonAsync($"/devices?machineName={machineName}&type={type}", string.Empty);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error creating device.");
        }
        
        var device = await response.Content.ReadFromJsonAsync<Device>();

        return device.Id;
    }
}