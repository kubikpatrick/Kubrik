using Kubrik.Services.Contracts;
using Kubrik.Web.Handlers;

namespace Kubrik.Web.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddPureHttpClient(this IServiceCollection services)
    {
        return services.AddScoped(sp => new HttpClient(new JwtAuthorizationHandler(sp.GetRequiredService<ITokenService>())));
    }
}