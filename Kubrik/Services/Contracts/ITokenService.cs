namespace Kubrik.Services.Contracts;

public interface ITokenService
{
    public Task<string?> GetTokenAsync();
    
    public Task SetTokenAsync(string token);

    public Task RemoveTokenAsync();
}