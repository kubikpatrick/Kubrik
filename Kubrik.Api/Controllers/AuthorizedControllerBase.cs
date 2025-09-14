using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Kubrik.Api.Controllers;

public abstract class AuthorizedControllerBase : ControllerBase
{
    public string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
}