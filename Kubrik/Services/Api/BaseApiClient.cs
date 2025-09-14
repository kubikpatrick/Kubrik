using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Kubrik.Services.Api;

public abstract class BaseApiClient
{
    private readonly HttpClient _http;

    protected BaseApiClient(HttpClient http)
    {
        _http = http;
    }

    protected async Task SendAsync(string url, HttpMethod method, object? o = null)
    {
        await SendAsync<object>(url, method, o);
    }

    protected async Task<T> SendAsync<T>(string url, HttpMethod method, object? o = null)
    {
        var response = await _http.SendAsync(new HttpRequestMessage(method, url)
        {
            Content = o is not null ? new StringContent(JsonSerializer.Serialize(o)) : null
        });

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException();
        }

        return await response.Content.ReadFromJsonAsync<T>();
    }

    public void SetAuthorizationToken(string token)
    {
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}