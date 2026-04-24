using IslandMerge.GameLogic;
using IslandMerge.Models;

namespace IslandMerge.Tests;

public class MergeEngineTests
{
    private static Item Item(ItemChain chain, int tier, int cell = 0, int playerId = 1) =>
        new() { Chain = chain, Tier = tier, CellIndex = cell, PlayerId = playerId };

    [Fact]
    public void TryMerge_SameChainSameTier_ReturnsNextTier()
    {
        var a = Item(ItemChain.Stone, 1, cell: 0);
        var b = Item(ItemChain.Stone, 1, cell: 1);

        var result = MergeEngine.TryMerge(a, b);

        Assert.True(result.Success);
        Assert.NotNull(result.Result);
        Assert.Equal(ItemChain.Stone, result.Result!.Chain);
        Assert.Equal(2, result.Result.Tier);
    }

    [Fact]
    public void TryMerge_DifferentChain_Fails()
    {
        var a = Item(ItemChain.Stone, 2);
        var b = Item(ItemChain.Wood, 2);

        var result = MergeEngine.TryMerge(a, b);

        Assert.False(result.Success);
        Assert.Equal("chain mismatch", result.Reason);
    }

    [Fact]
    public void TryMerge_DifferentTier_Fails()
    {
        var a = Item(ItemChain.Wood, 2);
        var b = Item(ItemChain.Wood, 3);

        var result = MergeEngine.TryMerge(a, b);

        Assert.False(result.Success);
        Assert.Equal("tier mismatch", result.Reason);
    }

    [Fact]
    public void TryMerge_AtMaxTier_Fails()
    {
        var a = Item(ItemChain.Crystal, BoardConstants.MaxTier);
        var b = Item(ItemChain.Crystal, BoardConstants.MaxTier);

        var result = MergeEngine.TryMerge(a, b);

        Assert.False(result.Success);
        Assert.Equal("max tier", result.Reason);
    }

    [Fact]
    public void TryMerge_NoneChain_Fails()
    {
        var a = Item(ItemChain.None, 1);
        var b = Item(ItemChain.None, 1);

        var result = MergeEngine.TryMerge(a, b);

        Assert.False(result.Success);
    }

    [Fact]
    public void TryMerge_ChainFromT1ToMaxTier_ProducesCorrectSequence()
    {
        // T1+T1 -> T2 ... T4+T4 -> T5. T5+T5 -> fail.
        for (var tier = 1; tier < BoardConstants.MaxTier; tier++)
        {
            var a = Item(ItemChain.Stone, tier);
            var b = Item(ItemChain.Stone, tier);
            var result = MergeEngine.TryMerge(a, b);
            Assert.True(result.Success, $"tier {tier} merge should succeed");
            Assert.Equal(tier + 1, result.Result!.Tier);
        }
    }

    [Fact]
    public void EnergyCostForMerge_MatchesDesignFormula()
    {
        Assert.Equal(1, MergeEngine.EnergyCostForMerge(1));
        Assert.Equal(2, MergeEngine.EnergyCostForMerge(2));
        Assert.Equal(2, MergeEngine.EnergyCostForMerge(3));
        Assert.Equal(3, MergeEngine.EnergyCostForMerge(4));
        Assert.Equal(3, MergeEngine.EnergyCostForMerge(5));
    }

    [Fact]
    public void TryMerge_PreservesChainType()
    {
        var a = Item(ItemChain.Crystal, 3);
        var b = Item(ItemChain.Crystal, 3);

        var result = MergeEngine.TryMerge(a, b);

        Assert.True(result.Success);
        Assert.Equal(ItemChain.Crystal, result.Result!.Chain);
    }

    [Fact]
    public void TryMerge_CopiesPlayerId()
    {
        var a = Item(ItemChain.Wood, 2, playerId: 42);
        var b = Item(ItemChain.Wood, 2, playerId: 42);

        var result = MergeEngine.TryMerge(a, b);

        Assert.Equal(42, result.Result!.PlayerId);
    }
}
