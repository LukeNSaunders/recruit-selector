using Microsoft.AspNetCore.Mvc;
using Server.Controllers;

namespace Server.Tests;

public class RecruitsControllerTests
{
    private readonly RecruitsController controller = new RecruitsController();

    // The smallest and largest allowed values should be accepted.
    [Theory]
    [InlineData(1, 1)]
    [InlineData(100, 100)]
    public void Get_LimitValues_AreAccepted(int recruits, int skip)
    {
        var response = controller.Get(recruits, skip);

        Assert.IsType<OkObjectResult>(response);
    }

    // Values outside the rules should be rejected.
    [Theory]
    [InlineData(null, 2)]    // number of recruits missing
    [InlineData(5, null)]    // number to skip missing
    [InlineData(0, 2)]       // too few recruits
    [InlineData(101, 2)]     // too many recruits
    [InlineData(5, 0)]       // skip must be at least 1
    [InlineData(5, -1)]      // negative skip
    [InlineData(5, 101)]     // skip too big
    public void Get_InvalidInput_IsRejected(int? recruits, int? skip)
    {
        var response = controller.Get(recruits, skip);

        Assert.IsType<BadRequestObjectResult>(response);
    }
}