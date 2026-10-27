using Server.Services;

namespace Server.Tests;

public class RecruitSelectorTests
{
    private readonly RecruitSelector selector = new RecruitSelector();

    // Basic example worked out by hand: recruit 3 is left.
    [Fact]
    public void SelectRecruits_ValidInput_ReturnsCorrectOrder()
    {
        var result = selector.SelectRecruits(5, 1);

        Assert.Equal(new[] { 2, 4, 1, 5 }, result.SelectedRecruits);
        Assert.Equal(3, result.FinalRecruit);
    }

    // A longer example where the sergeant goes around the circle several times.
    [Fact]
    public void SelectRecruits_LongerExample_ReturnsCorrectOrder()
    {
        var result = selector.SelectRecruits(7, 2);

        Assert.Equal(new[] { 3, 6, 2, 7, 5, 1 }, result.SelectedRecruits);
        Assert.Equal(4, result.FinalRecruit);
    }

    // One recruit: nobody is picked and they are the final recruit.
    [Fact]
    public void SelectRecruits_OneRecruit_NobodySelected()
    {
        var result = selector.SelectRecruits(1, 3);

        Assert.Empty(result.SelectedRecruits);
        Assert.Equal(1, result.FinalRecruit);
    }

    // Skip bigger than the circle: wraps around instead of crashing.
    [Fact]
    public void SelectRecruits_SkipMoreThanCircle_WrapsAround()
    {
        var result = selector.SelectRecruits(3, 5);

        Assert.Equal(new[] { 3, 2 }, result.SelectedRecruits);
        Assert.Equal(1, result.FinalRecruit);
    }

    // Skip equal to the circle size: one full lap back to the start.
    [Fact]
    public void SelectRecruits_SkipEqualsCircleSize_GoesFullLap()
    {
        var result = selector.SelectRecruits(3, 3);

        Assert.Equal(new[] { 1, 3 }, result.SelectedRecruits);
        Assert.Equal(2, result.FinalRecruit);
    }

    // Largest allowed values: everyone except one recruit is picked.
    [Fact]
    public void SelectRecruits_LargestAllowedValues_SelectsAllButOne()
    {
        var result = selector.SelectRecruits(100, 100);

        Assert.Equal(99, result.SelectedRecruits.Count);
    }
}