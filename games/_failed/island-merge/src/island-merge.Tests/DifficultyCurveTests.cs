using IslandMerge.GameLogic;

namespace IslandMerge.Tests;

public class DifficultyCurveTests
{
    [Fact]
    public void MergeGoal_L1_Returns3()
    {
        Assert.Equal(3, DifficultyCurve.MergeGoal(1));
    }

    [Fact]
    public void MergeGoal_L10_Returns5()
    {
        Assert.Equal(5, DifficultyCurve.MergeGoal(10));
    }

    [Fact]
    public void MergeGoal_L50_Returns13()
    {
        Assert.Equal(13, DifficultyCurve.MergeGoal(50));
    }

    [Fact]
    public void TimeTolerance_L1_Returns180()
    {
        Assert.Equal(180, DifficultyCurve.TimeToleranceSeconds(1));
    }

    [Fact]
    public void TimeTolerance_ClampsToMinimum90()
    {
        Assert.Equal(90, DifficultyCurve.TimeToleranceSeconds(500));
    }

    [Fact]
    public void RewardsScaleUpWithLevel()
    {
        Assert.True(DifficultyCurve.XpRewardForLevel(20) > DifficultyCurve.XpRewardForLevel(1));
        Assert.True(DifficultyCurve.CoinRewardForLevel(20) > DifficultyCurve.CoinRewardForLevel(1));
    }
}
