using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Route("api/recruits")]
public class RecruitsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get(int n, int k)
    {
        if (n < 1)
        {
            return BadRequest("n must be at least 1");
        }

        if (k < 0)
        {
            return BadRequest("k cannot be negative");
        }

        var selector = new RecruitSelector();
        var result = selector.SelectRecruits(n, k);

        return Ok(result);
    }
}