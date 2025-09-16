using System.Net.Http.Json;

using Kubrik.Models.Http;
using Kubrik.Services.Contracts;

namespace Kubrik.Daemon.Services;

public sealed class TokenService : ITokenService
{
    private readonly HttpClient _http;
 
    private readonly IConfiguration _configuration;
    
    public TokenService(HttpClient http, IConfiguration configuration)
    {
        _http = http;
        _configuration = configuration;
    }
    
    public async Task<string?> GetTokenAsync()
    {
        var response = await _http.PostAsJsonAsync("/authentication/login", new LoginRequest
        {
            Email = _configuration.GetValue<string>("Credentials:Email"),
            Password = _configuration.GetValue<string>("Credentials:Password")
        });

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        
        return (await response.Content.ReadFromJsonAsync<TokenResponse>()).AccessToken;
    }

    public async Task SetTokenAsync(string token)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveTokenAsync()
    {
        throw new NotImplementedException();
    }
}