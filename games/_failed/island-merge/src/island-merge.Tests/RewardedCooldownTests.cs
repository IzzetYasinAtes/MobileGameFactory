using IslandMerge.Services;

namespace IslandMerge.Tests;

public sealed class RewardedCooldownTests
{
    [Fact]
    public void IsReady_DefaultTrue_BeforeAnyShow()
    {
        var cd = new RewardedCooldown();
        Assert.True(cd.IsReady(AdPlacement.EnergyRefill));
        Assert.Null(cd.TimeLeft(AdPlacement.EnergyRefill));
    }

    [Fact]
    public void NotifyShown_BlocksSameSlotForThirtySeconds()
    {
        DateTimeOffset now = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var cd = new RewardedCooldown(() => now);
        cd.NotifyShown(AdPlacement.EnergyRefill);

        Assert.False(cd.IsReady(AdPlacement.EnergyRefill));
        now = now.AddSeconds(29);
        Assert.False(cd.IsReady(AdPlacement.EnergyRefill));
        now = now.AddSeconds(2);
        Assert.True(cd.IsReady(AdPlacement.EnergyRefill));
    }

    [Fact]
    public void DifferentPlacements_AreIndependent()
    {
        DateTimeOffset now = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var cd = new RewardedCooldown(() => now);
        cd.NotifyShown(AdPlacement.EnergyRefill);

        Assert.False(cd.IsReady(AdPlacement.EnergyRefill));
        Assert.True(cd.IsReady(AdPlacement.LootDouble));
    }
}
