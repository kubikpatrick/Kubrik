using System.Text;

using Kubrik.Api.Data;
using Kubrik.Api.Services;
using Kubrik.Api.Services.Contracts;
using Kubrik.Models.Identity;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Kubrik.Api.Services.Managers;

namespace Kubrik.Api.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

        return services;
    }

    public static IServiceCollection AddManagers(this IServiceCollection services)
    {
        services.AddScoped<DeviceManager>();

        return services;
    }

    public static IServiceCollection AddDefaultCors(this IServiceCollection services)
    {
        services.AddCors(cors =>
        {
            cors.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyOrigin();
                policy.AllowAnyMethod();
            });
        });

        return services;
    }
    
    public static IServiceCollection AddCurrentUserIdAccessor(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserIdAccessor, CurrentUserIdAccessor>();
        
        return services;
    }
    
    public static IServiceCollection AddTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<JwtService>();
        services.AddAuthentication(options => 
        { 
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
            };
            
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var token = context.Request.Query[DataRegisteredNames.AccessToken];
                    var path = context.HttpContext.Request.Path;
                    
                    if (!string.IsNullOrEmpty(token) && path.StartsWithSegments("/hubs"))
                    {
                        context.Token = token;
                    }
                    
                    return Task.CompletedTask;
                },
                
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                    
                    return Task.CompletedTask;
                },
            };
        });
        
        return services;
    }
}