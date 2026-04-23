using SQLite;

namespace IslandMerge.Models;

public class Player
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string PetChoice { get; set; } = "Momo";

    public int TotalXp { get; set; }

    public int SoftCurrency { get; set; }

    public int HardCurrency { get; set; }

    public int Energy { get; set; } = 100;

    public int EnergyMax { get; set; } = 100;

    /// <summary>Son enerji yenileme zamaninin epoch seconds'i (UTC). Arka plan hesaplamasi icin.</summary>
    public long LastEnergyRefillUtc { get; set; }

    public int CurrentLevel { get; set; } = 1;

    public BiomeId CurrentBiome { get; set; } = BiomeId.TropicalForest;

    public int StreakDays { get; set; }

    public string LastLoginDateIso { get; set; } = string.Empty;

    public bool RemoveAdsPurchased { get; set; }
}
