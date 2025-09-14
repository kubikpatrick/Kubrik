using System.Net.Http.Json;

using Kubrik.Models.Http;
using Kubrik.Services.Contracts;

using Microsoft.AspNetCore.Components;

namespace Kubrik.Web.Pages;

public partial class SignUp : ComponentBase
{
    private readonly HttpClient _http;
    private readonly NavigationManager _navigation;
    
    private readonly IServerUrlService _serverUrlService;
    
    public SignUp(HttpClient http, NavigationManager navigation, IServerUrlService serverUrlService)
    {
        _http = http;
        _navigation = navigation;
        _serverUrlService = serverUrlService;
    }
    
    private SignUpRequest request = new SignUpRequest();

    protected override async Task OnInitializedAsync()
    {
        
    }

    private async Task Submit()
    {
        string? url = await _serverUrlService.GetServerUrlAsync();
        
        var response = await _http.PostAsJsonAsync($"{url}/authentication/sign-up", request);
        if (response.IsSuccessStatusCode)
        {
            _navigation.NavigateTo("/login", true);
        }
    }
}