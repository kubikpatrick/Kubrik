using Microsoft.AspNetCore.Mvc;

namespace Kubrik.Api.Controllers;

[Route("[controller]")]
public sealed class HealthController : ControllerBase
{
    public HealthController()
    {
        
    }

    [HttpGet]
    public ActionResult Index()
    {
        return Ok();
    }
}