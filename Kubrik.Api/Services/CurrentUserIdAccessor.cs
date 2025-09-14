using System.Security.Claims;

using Kubrik.Api.Services.Contracts;

namespace Kubrik.Api.Services;

public sealed class CurrentUserIdAccessor : ICurrentUserIdAccessor
{
    public CurrentUserIdAccessor(IHttpContextAccessor accessor)
    {
        UserId = accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new ArgumentNullException(nameof(accessor));
    }
    
    public string UserId { get; }
}