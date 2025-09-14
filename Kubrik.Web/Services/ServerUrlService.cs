using Blazored.LocalStorage;

using Kubrik.Services.Contracts;

namespace Kubrik.Web.Services;

public sealed class ServerUrlService : IServerUrlService
{
    private readonly ILocalStorageService _localStorage;

    public ServerUrlService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<string?> GetServerUrlAsync()
    {
        return await _localStorage.GetItemAsStringAsync(DataRegisteredNames.ServerUrl);
    }

    public async Task SetServerUrlAsync(string url)
    {
        if (url.EndsWith('/'))
        {
            url = url[..^1];
        }
        
        await _localStorage.SetItemAsStringAsync(DataRegisteredNames.ServerUrl, url);
    }

    public async Task RemoveServerUrlAsync()
    {
        await _localStorage.RemoveItemAsync(DataRegisteredNames.ServerUrl);       
    }
}