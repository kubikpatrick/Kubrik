using Kubrik.Api.Hubs;

namespace Kubrik.Api.Extensions;

public static class AspNetCoreExtensions
{
    public static WebApplication MapHubs(this WebApplication app)
    {
        app.MapHub<LocationHub>("/hubs/Location");
        
        return app;
    }
    
}