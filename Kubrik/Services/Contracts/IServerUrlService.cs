namespace Kubrik.Services.Contracts;

public interface IServerUrlService
{
    public Task<string?> GetServerUrlAsync();
    
    public Task SetServerUrlAsync(string url);

    public Task RemoveServerUrlAsync();
}