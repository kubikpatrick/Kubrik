using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Kubrik.Models;
using Kubrik.Models.Devices;
using Kubrik.Api.Services.Managers;

namespace Kubrik.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public sealed class DevicesController : AuthorizedControllerBase
{
    private readonly DeviceManager _deviceManager;
    
    public DevicesController(DeviceManager deviceManager)
    {
        _deviceManager = deviceManager;
    }
    
    [HttpGet]
    public async Task<ActionResult<Device[]>> Index()
    {
        var devices = await _deviceManager.GetAllAsync(CurrentUserId);
        
        return Ok(devices);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Device>> Get([FromRoute] string id)
    {
        var device = await _deviceManager.FindByIdAsync(id);
        if (device is null || device.UserId != CurrentUserId)
        {
            return NotFound();
        }
        
        return Ok(device);
    }

    [HttpPost]
    public async Task<ActionResult<Device>> Create([FromQuery] string name, [FromQuery] DeviceType type)
    {
        var device = new Device
        {
            Name = name,
            Type = type,
            UserId = CurrentUserId
        };
        
        var result = await _deviceManager.CreateAsync(device);
        if (!result.Succeeded)
        {
            return Problem(result.Message);
        }
        
        return Ok(device);
    }

    [HttpPatch("{id:guid}/location")]
    public async Task<ActionResult> Location([FromRoute] string id, [FromBody] Location location)
    {
        var device = await _deviceManager.FindByIdAsync(id);
        if (device is null || device.UserId != CurrentUserId)
        {
            return NotFound();
        }
        
        await _deviceManager.UpdateLocationAsync(device, location);

        return Ok();
    }
}