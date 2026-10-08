using Microsoft.AspNetCore.Mvc;
using Server.Controllers;
using Server.Models;

namespace Server.Tests;

public class RecruitsControllerTests
{
    private readonly RecruitsController controller = new RecruitsController();

    // Valid input should return 200 OK with the correct answer.
    [Fact]
    public void Get_ValidInput_ReturnsCorrectResult()
    {
        var response = controller.Get(5, 1);

        var ok = Assert.IsType<OkObjectResult>(response);
        var result = Assert.IsType<RecruitResult>(ok.Value);
        Assert.Equal(3, result.FinalRecruit);
    }

    // The smallest and largest allowed values should still work.
    [Theory]
    [InlineData(1, 0)]       // smallest allowed
    [InlineData(100, 100)]   // largest allowed
    public void Get_LimitValues_ReturnsOk(int n, int k)
    {
        var response = controller.Get(n, k);

        Assert.IsType<OkObjectResult>(response);
    }

    // Bad input should be rejected with 400 Bad Request.
    [Theory]
    [InlineData(null, 2)]    // number of recruits missing
    [InlineData(5, null)]    // number of recruits to skip missing
    [InlineData(0, 2)]       // too few recruits
    [InlineData(101, 2)]     // too many recruits
    [InlineData(5, -1)]      // negative skip
    [InlineData(5, 101)]     // skip too big
    public void Get_InvalidInput_ReturnsBadRequest(int? n, int? k)
    {
        var response = controller.Get(n, k);

        Assert.IsType<BadRequestObjectResult>(response);
    }

    // Missing input should tell the user what to enter.
    [Fact]
    public void Get_MissingInput_ReturnsHelpfulMessage()
    {
        var response = controller.Get(null, 2);

        var badRequest = Assert.IsType<BadRequestObjectResult>(response);
        Assert.Equal(
            "Please enter both the number of recruits and the number to skip.",
            badRequest.Value);
    }
}