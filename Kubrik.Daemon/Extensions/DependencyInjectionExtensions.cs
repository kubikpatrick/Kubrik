using System.Net.Http.Headers;

namespace Kubrik.Daemon.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddHttpClient(this IServiceCollection services)
    {
        services.AddSingleton<HttpClient>(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            
            return new HttpClient
            {
                BaseAddress = new Uri(configuration["Credentials:ServerUrl"])
            };
        });
        
        return services;
    }
}