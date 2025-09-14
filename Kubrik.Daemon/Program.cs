using Kubrik.Daemon.Extensions;
using Kubrik.Daemon.Services;
using Kubrik.Daemon.Workers;
using Kubrik.Services.Contracts;

namespace Kubrik.Daemon;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddHttpClient();
        builder.Services.AddSingleton(new SemaphoreSlim(1, 1));
        builder.Services.AddTransient<ITokenService, TokenService>();
        builder.Services.AddSingleton<DeviceIdService>();
        builder.Services.AddHostedService<JwtRefreshWorker>();
        builder.Services.AddHostedService<LocationWorker>();

        var host = builder.Build();
        
        host.Run();
    }
}