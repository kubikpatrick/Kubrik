using System.Net.Http.Headers;
using System.Net.Http.Json;

using Kubrik.Models.Http;
using Kubrik.Services.Contracts;

namespace Kubrik.Daemon.Workers;

public sealed class JwtRefreshWorker : BackgroundService
{
    private readonly HttpClient _http;
    private readonly SemaphoreSlim _lock;
    
    private readonly ITokenService _tokenService;
    
    public JwtRefreshWorker(HttpClient http, SemaphoreSlim @lock, ITokenService tokenService)
    {
        _http = http;
        _lock = @lock;
        _tokenService = tokenService;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);

        await RefreshTokenAsync(cancellationToken);
        
        _lock.Release();
        
        await base.StartAsync(cancellationToken);
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await RefreshTokenAsync(stoppingToken);
            
            await Task.Delay(TimeSpan.FromMinutes(14), stoppingToken);
        }
    }

    private async Task RefreshTokenAsync(CancellationToken cancellationToken)
    {
        string? token = await _tokenService.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        else
        {
            throw new InvalidOperationException("Credentials are invalid.");
        }
    }
}