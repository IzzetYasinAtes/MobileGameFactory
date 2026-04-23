using IslandMerge.GameLogic;
using IslandMerge.Models;

namespace IslandMerge.Tests;

public class EnergySystemTests
{
    private static Player MakePlayer(int level, int energy = 100, int max = 100, long refillEpoch = 0) =>
        new()
        {
            Id = 1,
            CurrentLevel = level,
            Energy = energy,
            EnergyMax = max,
            LastEnergyRefillUtc = refillEpoch,
        };

    [Fact]
    public void Consume_BelowBarrierLevel_ImmediatelyRefillsToMax()
    {
        var p = MakePlayer(level: 5, energy: 50);
        var result = EnergySystem.Consume(p, 10, nowUtc: 100);

        Assert.Equal(p.EnergyMax, result);
        Assert.Equal(100, p.LastEnergyRefillUtc);
    }

    [Fact]
    public void Consume_AboveBarrierLevel_DecrementsEnergy()
    {
        var p = MakePlayer(level: BoardConstants.EnergyBarrierLevel, energy: 80);
        var result = EnergySystem.Consume(p, 5, nowUtc: 100);

        Assert.Equal(75, result);
    }

    [Fact]
    public void Consume_ZeroCost_NoChange()
    {
        var p = MakePlayer(level: 20, energy: 40);
        var result = EnergySystem.Consume(p, 0, nowUtc: 100);

        Assert.Equal(40, result);
    }

    [Fact]
    public void Regenerate_BelowBarrier_KeepsAtMax()
    {
        var p = MakePlayer(level: 5, energy: 30);
        var result = EnergySystem.Regenerate(p, nowUtc: 100);

        Assert.Equal(p.EnergyMax, result);
    }

    [Fact]
    public void Regenerate_AboveBarrier_RefillsByElapsedCooldownUnits()
    {
        var p = MakePlayer(level: 20, energy: 0, refillEpoch: 0);
        // 3 tam cooldown periyodu kadar gectiyse +3 enerji.
        var now = BoardConstants.EnergyCooldownSeconds * 3L;
        var result = EnergySystem.Regenerate(p, nowUtc: now);

        Assert.Equal(3, result);
    }

    [Fact]
    public void Regenerate_AboveBarrier_DoesNotExceedMax()
    {
        var p = MakePlayer(level: 20, energy: 99, refillEpoch: 0);
        var now = BoardConstants.EnergyCooldownSeconds * 10L;
        var result = EnergySystem.Regenerate(p, nowUtc: now);

        Assert.Equal(100, result);
    }

    [Fact]
    public void Grant_AddsButCapsAtMax()
    {
        var p = MakePlayer(level: 20, energy: 80);
        var result = EnergySystem.Grant(p, 50);

        Assert.Equal(100, result);
    }

    [Fact]
    public void GrantOvercap_AllowsAboveMax()
    {
        var p = MakePlayer(level: 20, energy: 50);
        var result = EnergySystem.GrantOvercap(p, 500);

        Assert.Equal(550, result);
    }
}
