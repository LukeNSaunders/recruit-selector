using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/recruits")]
public class RecruitsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get(int n, int k)
    {
        return Ok(new { n, k });
    }
}