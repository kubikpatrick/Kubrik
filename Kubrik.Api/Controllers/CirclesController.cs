using Kubrik.Api.Services.Managers;
using Kubrik.Models.Circles;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kubrik.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public sealed class CirclesController : AuthorizedControllerBase
{
    private readonly CircleManager _circleManager;

    public CirclesController(CircleManager circleManager)
    {
        _circleManager = circleManager;
    }

    [HttpGet]
    public async Task<ActionResult<List<Circle>>> Index()
    {
        return Ok(Array.Empty<Circle>());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Circle>> Get([FromRoute] string id)
    {
        var circle = await _circleManager.FindByIdAsync(id);
        if (circle is null || circle.UserId != CurrentUserId)
        {
            return NotFound();
        }

        return Ok(circle);
    }

    [HttpPost]
    public async Task<ActionResult<Circle>> Create([FromQuery] string name)
    {
        var circle = new Circle
        {
            Name = name,
            CreatedAt = DateTime.UtcNow,
            UserId = CurrentUserId
        };

        var result = await _circleManager.CreateAsync(circle);
        if (!result.Succeeded)
        {
            return Problem(result.Message, statusCode: result.HttpCode);
        }
        
        return Ok(circle);
    }
    
    [HttpPost("{id:guid}/members")]
    public async Task<ActionResult> Add([FromRoute] string id, [FromQuery] string userId)
    {
        var circle = await _circleManager.FindByIdAsync(id);
        if (circle is null || circle.UserId != CurrentUserId)
        {
            return NotFound();
        }
        
        var result = await _circleManager.AddMemberAsync(circle, new Member
        {
            Nickname = string.Empty,
            CreatedAt = DateTime.UtcNow,
            CircleId = id,
            UserId = userId
        });
        
        if (!result.Succeeded)
        {
            return Problem(result.Message, statusCode: result.HttpCode);
        }
        
        return Ok();
    }

    [HttpDelete("{id:guid}/members/{memberId:guid}")]
    public async Task<ActionResult> Remove([FromRoute] string id, [FromRoute] string memberId)
    {
        var circle = await _circleManager.FindByIdAsync(id);
        if (circle is null || circle.UserId != CurrentUserId)
        {
            return NotFound();
        }
        
        var result = await _circleManager.RemoveMemberAsync(circle, memberId);
        if (!result.Succeeded)
        {
            return Problem(result.Message, statusCode: result.HttpCode);
        }

        return Ok();
    }
}