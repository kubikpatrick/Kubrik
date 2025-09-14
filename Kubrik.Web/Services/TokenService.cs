using Blazored.LocalStorage;
using Kubrik.Services;
using Kubrik.Services.Contracts;

namespace Kubrik.Web.Services;

public sealed class TokenService : ITokenService
{
    private readonly ILocalStorageService _localStorage;
    
    public TokenService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }
    
    public async Task<string?> GetTokenAsync()
    {
        return await _localStorage.GetItemAsStringAsync(DataRegisteredNames.AccessToken);
    }

    public async Task SetTokenAsync(string token)
    {
        
        await _localStorage.SetItemAsStringAsync(DataRegisteredNames.AccessToken, token);
    }

    public async Task RemoveTokenAsync()
    {
        await _localStorage.RemoveItemAsync(DataRegisteredNames.AccessToken);
    }
}