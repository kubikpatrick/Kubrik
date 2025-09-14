using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Kubrik.Services.Contracts;

using Microsoft.AspNetCore.Components.Authorization;

namespace Kubrik.Web.Services;

public sealed class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ITokenService _tokenService;
    
    public JwtAuthenticationStateProvider(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }
    
    private static AuthenticationState Anonymous => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string? token = await _tokenService.GetTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            return Anonymous;
        }

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            if (jwt.ValidTo < DateTime.UtcNow)
            {
                await _tokenService.RemoveTokenAsync();
                
                return Anonymous;
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(jwt.Claims, "jwt")));
        }
        catch
        {
            return Anonymous;
        }
    }

    public async Task NotifyAuthenticationAsync(string token)
    {
        await _tokenService.SetTokenAsync(token);
        
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task NotifyLogoutAsync()
    {
        await _tokenService.RemoveTokenAsync();
        
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}