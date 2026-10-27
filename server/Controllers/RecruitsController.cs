using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Authorize]
[Route("api/recruits")]
public class RecruitsController : ControllerBase
{
    private const int MaxRecruits = 100;
    private const int MaxSkip = 100;
    [HttpGet]
    public IActionResult Get(int? recruits, int? skip)
    {
        if (recruits == null || skip == null)
        {
            return BadRequest("Please enter both the number of recruits and the number to skip.");
        }

        if (recruits < 1)
        {
            return BadRequest("At least one recruit is required.");
        }

        if (recruits > MaxRecruits)
        {
            return BadRequest($"The number of recruits can't be more than {MaxRecruits:N0}.");
        }

        if (skip < 1)
        {
            return BadRequest("At least one recruit must be skipped.");
        }

        if (skip > MaxSkip)
        {
            return BadRequest($"The number to skip can't be more than {MaxSkip:N0}.");
        }

        try
        {
            var selector = new RecruitSelector();
            var result = selector.SelectRecruits(recruits.Value, skip.Value);
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "Something went wrong on our end, please try again.");
        }
    }
}