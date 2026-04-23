using IslandMerge.GameLogic;
using IslandMerge.Models;

namespace IslandMerge.Tests;

public class QuestEvaluatorTests
{
    private static Quest StoneT2Quest(int needed = 2) => new()
    {
        Id = 1,
        PlayerId = 1,
        Biome = BiomeId.TropicalForest,
        LevelId = 1,
        TargetChain = ItemChain.Stone,
        TargetTier = 2,
        TargetQuantity = needed,
        RewardXp = 10,
        RewardCoin = 20,
    };

    [Fact]
    public void Contribute_MatchingItem_IncrementsDelivered()
    {
        var q = StoneT2Quest(2);
        var item = new Item { Chain = ItemChain.Stone, Tier = 2 };

        Assert.True(QuestEvaluator.TryContribute(q, item));
        Assert.Equal(1, q.DeliveredQuantity);
    }

    [Fact]
    public void Contribute_WrongTier_Ignored()
    {
        var q = StoneT2Quest();
        var item = new Item { Chain = ItemChain.Stone, Tier = 3 };

        Assert.False(QuestEvaluator.TryContribute(q, item));
        Assert.Equal(0, q.DeliveredQuantity);
    }

    [Fact]
    public void Contribute_WrongChain_Ignored()
    {
        var q = StoneT2Quest();
        var item = new Item { Chain = ItemChain.Wood, Tier = 2 };

        Assert.False(QuestEvaluator.TryContribute(q, item));
    }

    [Fact]
    public void Contribute_AlreadyFull_Ignored()
    {
        var q = StoneT2Quest(1);
        q.DeliveredQuantity = 1;
        var item = new Item { Chain = ItemChain.Stone, Tier = 2 };

        Assert.False(QuestEvaluator.TryContribute(q, item));
    }

    [Fact]
    public void TryComplete_WhenQuotaMet_MarksCompleted()
    {
        var q = StoneT2Quest(1);
        q.DeliveredQuantity = 1;

        Assert.True(QuestEvaluator.TryComplete(q));
        Assert.True(q.Completed);
    }

    [Fact]
    public void TryComplete_UnderQuota_Fails()
    {
        var q = StoneT2Quest(2);
        q.DeliveredQuantity = 1;

        Assert.False(QuestEvaluator.TryComplete(q));
        Assert.False(q.Completed);
    }
}
