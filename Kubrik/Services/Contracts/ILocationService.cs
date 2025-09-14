using Kubrik.Models;

namespace Kubrik.Services.Contracts;

public interface ILocationService
{
    public Task<Location?> GetCurrentLocationAsync();
}