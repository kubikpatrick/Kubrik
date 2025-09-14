using System.Net.Http.Headers;
using Kubrik.Services.Contracts;

namespace Kubrik.Services.Api;

public sealed class ApiClient : IApiClient
{
    private readonly HttpClient _http;
    
    public ApiClient(HttpClient http)
    {
        _http = http;
        
        Authentication = new AuthenticationApiClient(http);
        Devices = new DeviceApiClient(http);
    }
    
    public AuthenticationApiClient Authentication { get; }
    public DeviceApiClient Devices { get; }

    public void SetAuthorizationToken(string token)
    {
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}