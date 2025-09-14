using System.Net.Http.Headers;

using Kubrik.Services;
using Kubrik.Services.Contracts;

namespace Kubrik.Web.Handlers;

public sealed class JwtAuthorizationHandler : DelegatingHandler
{
    private readonly ITokenService _tokenService;
    
    public JwtAuthorizationHandler(ITokenService tokenService)
    {
        InnerHandler = new HttpClientHandler();
        
        _tokenService = tokenService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string? token = await _tokenService.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        
        return await base.SendAsync(request, cancellationToken);
    }
}