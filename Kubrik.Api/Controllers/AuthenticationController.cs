using Kubrik.Api.Extensions;
using Kubrik.Api.Services;
using Kubrik.Models.Http;
using Kubrik.Models.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Kubrik.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AuthenticationController : ControllerBase
{
    private readonly JwtService _jwt;
    private readonly UserManager<User> _userManager;
    
    public AuthenticationController(JwtService jwt, UserManager<User> userManager)
    {
        _jwt = jwt;
        _userManager = userManager;
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return NotFound();
        }

        bool valid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!valid)
        {
            return Unauthorized();
        }
        
        return Ok(new TokenResponse
        {
            AccessToken = _jwt.GenerateToken(user),
        });
    }
    
    [HttpPost("sign-up")]
    public async Task<ActionResult> SignUp([FromBody] SignUpRequest request)
    {
        bool exists = await _userManager.FindByEmailAsync(request.Email) is not null;
        if (exists)
        {
            return Conflict();
        }

        var user = new User
        {
            Email = request.Email,
            UserName = request.Email,
            FirstName = string.Empty,
            LastName = string.Empty,
            CreatedAt = DateTime.UtcNow
        };
        
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return BadRequest();
        }
        
        return Created();
    }
}