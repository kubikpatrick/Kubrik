using Blazored.LocalStorage;

using Kubrik.Services.Contracts;
using Kubrik.Web.Extensions;
using Kubrik.Web.Services;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Kubrik.Web;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
        
        builder.Services.AddAuthorizationCore();
        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddGeolocationServices();
        builder.Services.AddPureHttpClient();
        builder.Services.AddLucideIcons();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<ILocationService, LocationService>();
        builder.Services.AddScoped<IServerUrlService, ServerUrlService>();
        builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();

        await builder.Build().RunAsync();
    }
}