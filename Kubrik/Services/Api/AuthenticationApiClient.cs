using Kubrik.Models.Http;

namespace Kubrik.Services.Api;

public sealed class AuthenticationApiClient : BaseApiClient
{
    public AuthenticationApiClient(HttpClient http) : base(http)
    {
        
    }

    public async Task<TokenResponse> LoginAsync(string email, string password)
    {
        return await SendAsync<TokenResponse>("/authentication/login", HttpMethod.Post, new LoginRequest
        {
            Email = email,
            Password = password
        });
    }

    public async Task SignUpAsync(string email, string password, string confirmPassword)
    {
        await SendAsync("/authentication/sign-up", HttpMethod.Post, new SignUpRequest
        {
            Email = email,
            Password = password,
            ConfirmPassword = confirmPassword
        });
    }

    public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
    {
        return await SendAsync<TokenResponse>("/authentication/refresh", HttpMethod.Post, new RefreshRequest(refreshToken));
    }
}