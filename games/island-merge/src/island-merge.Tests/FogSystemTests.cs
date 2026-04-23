using IslandMerge.GameLogic;
using IslandMerge.Models;

namespace IslandMerge.Tests;

public class FogSystemTests
{
    [Fact]
    public void FirstReveal_OpensCenterTile()
    {
        var fog = new FogSystem(BoardConstants.FogTileCount);

        var idx = fog.RevealNext();

        Assert.NotNull(idx);
        Assert.Equal(BoardConstants.FogTileCount / 2, idx);
        Assert.True(fog.IsRevealed(idx!.Value));
    }

    [Fact]
    public void RegisterMerge_RevealsEveryThirdCall()
    {
        var fog = new FogSystem(BoardConstants.FogTileCount);
        var revealed1 = fog.RegisterMergeAndMaybeReveal();
        var revealed2 = fog.RegisterMergeAndMaybeReveal();
        var revealed3 = fog.RegisterMergeAndMaybeReveal();

        Assert.Null(revealed1);
        Assert.Null(revealed2);
        Assert.NotNull(revealed3);
        Assert.Equal(1, fog.RevealedCount);
    }

    [Fact]
    public void RevealBatch_OpensRequestedTiles()
    {
        var fog = new FogSystem(BoardConstants.FogTileCount);
        var opened = fog.RevealBatch(BoardConstants.QuestBonusFogReveal);

        Assert.Equal(BoardConstants.QuestBonusFogReveal, opened.Count);
        Assert.Equal(BoardConstants.QuestBonusFogReveal, fog.RevealedCount);
    }

    [Fact]
    public void LoadFromMask_RestoresState()
    {
        var mask = new bool[BoardConstants.FogTileCount];
        mask[0] = true;
        mask[1] = true;

        var fog = new FogSystem(BoardConstants.FogTileCount);
        fog.LoadFromMask(mask);

        Assert.Equal(2, fog.RevealedCount);
        Assert.True(fog.IsRevealed(0));
        Assert.True(fog.IsRevealed(1));
        Assert.False(fog.IsRevealed(2));
    }

    [Fact]
    public void RevealNext_AfterFull_ReturnsNull()
    {
        var fog = new FogSystem(BoardConstants.FogTileCount);
        // Maskeyi tam dolu yukle.
        var mask = new bool[BoardConstants.FogTileCount];
        for (var i = 0; i < mask.Length; i++)
        {
            mask[i] = true;
        }
        fog.LoadFromMask(mask);

        var idx = fog.RevealNext();
        Assert.Null(idx);
    }
}
