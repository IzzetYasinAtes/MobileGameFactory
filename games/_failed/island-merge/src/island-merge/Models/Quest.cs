using SQLite;

namespace IslandMerge.Models;

public class Quest
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int PlayerId { get; set; }

    public BiomeId Biome { get; set; }

    public int LevelId { get; set; }

    public ItemChain TargetChain { get; set; }

    public int TargetTier { get; set; }

    public int TargetQuantity { get; set; }

    public int DeliveredQuantity { get; set; }

    public int RewardXp { get; set; }

    public int RewardCoin { get; set; }

    public bool Completed { get; set; }

    [Ignore]
    public bool CanComplete => !Completed && DeliveredQuantity >= TargetQuantity;
}
