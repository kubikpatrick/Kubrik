using Kubrik.Api.Data;
using Kubrik.Api.Extensions;

using Microsoft.EntityFrameworkCore;

namespace Kubrik.Api;

public sealed class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddSignalR();
        builder.Services.AddMemoryCache();
        builder.Services.AddIdentity();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddCurrentUserIdAccessor();
        builder.Services.AddManagers();
        builder.Services.AddDefaultCors();
        builder.Services.AddTokenAuthentication(builder.Configuration);
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("Default"));
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseCors();
        app.UseAuthorization();
        app.MapControllers();
        app.MapHubs();
        
        app.Run();
    }
}