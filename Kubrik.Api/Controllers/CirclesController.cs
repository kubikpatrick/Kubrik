using Kubrik.Api.Data;
using Kubrik.Models.Circles;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Kubrik.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public sealed class CirclesController : AuthorizedControllerBase
{
    private readonly ApplicationDbContext _context;
    
    private readonly IMemoryCache _cache;
    
    public CirclesController(ApplicationDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    [HttpGet]
    public async Task<ActionResult<List<Circle>>> Index()
    {
        var circles = await _context.Circles.Where(c => c.UserId == CurrentUserId).ToListAsync();
        
        return Ok(circles);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Circle>> Get([FromRoute] string id)
    {
        var circle = await _context.Circles.FindAsync(id);
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
        
        await _context.Circles.AddAsync(circle);
        await _context.SaveChangesAsync();
        
        return Ok(circle);
    }
    
    [HttpPost("{id:guid}/members")]
    public async Task<ActionResult> AddMember([FromRoute] string id, [FromBody] Member member)
    {
        var circle = await _context.Circles.Include(c => c.Members).FirstOrDefaultAsync(c => c.Id == id && c.UserId == CurrentUserId);
        if (circle is null)
        {
            return NotFound();
        }

        if (circle.Members.Any(m => m.UserId == member.UserId))
        {
            return Conflict();
        }

        circle.Members.Add(member);   
            
        await _context.SaveChangesAsync();
        
        return Ok();
    }
}