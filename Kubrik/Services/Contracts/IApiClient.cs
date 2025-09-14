using Kubrik.Services.Api;

namespace Kubrik.Services.Contracts;

public interface IApiClient
{
    public DeviceApiClient Devices { get; }
    
    public void SetAuthorizationToken(string token);
}