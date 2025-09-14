using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

using Kubrik.Models.Http;
using Kubrik.Services.Contracts;
using Kubrik.Web.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Kubrik.Web.Pages;

public partial class Login : ComponentBase
{
    private readonly JwtAuthenticationStateProvider _authenticationStateProvider;
    private readonly HttpClient _http;
    private readonly NavigationManager _navigation;
    
    private readonly IServerUrlService _serverUrlService;

    public Login(AuthenticationStateProvider authenticationStateProvider, HttpClient http, NavigationManager navigation, IServerUrlService serverUrlService)
    {
        _authenticationStateProvider = authenticationStateProvider as JwtAuthenticationStateProvider;
        _http = http;
        _navigation = navigation;
        _serverUrlService = serverUrlService;
    }

    private LoginRequest request = new LoginRequest();
    
    [Required(ErrorMessage = "Server URL is required.")]
    [Url(ErrorMessage = "URL is not valid.")]
    public string? ServerUrl { get; set; }

    protected override async Task OnInitializedAsync()
    {
        ServerUrl = await _serverUrlService.GetServerUrlAsync();
    }

    private async Task Submit()
    {
        var response = await _http.PostAsJsonAsync($"{ServerUrl}/authentication/login", request);
        if (!response.IsSuccessStatusCode)
        {
            
        }
        
        var content = await response.Content.ReadFromJsonAsync<TokenResponse>();

        await _authenticationStateProvider.NotifyAuthenticationAsync(content.AccessToken);
        await _serverUrlService.SetServerUrlAsync(ServerUrl);

        _navigation.NavigateTo("/");
    }
}