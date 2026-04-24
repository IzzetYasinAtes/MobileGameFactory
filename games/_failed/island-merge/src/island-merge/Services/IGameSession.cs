using IslandMerge.Models;

namespace IslandMerge.Services;

/// <summary>
/// Tek oturum icin oyuncu + board + quest + fog'un koordinasyonu.
/// ViewModel'ler bunu cagirir. Platform bilinciz.
/// </summary>
public interface IGameSession
{
    Player Player { get; }

    IReadOnlyList<Item> BoardItems { get; }

    IReadOnlyList<Quest> ActiveQuests { get; }

    IReadOnlyList<bool> FogMask { get; }

    int LastRevealedTileIndex { get; }

    Task LoadAsync(CancellationToken ct = default);

    Task<MergeOutcome> TryMergeAsync(int sourceCellIndex, int targetCellIndex, CancellationToken ct = default);

    Task<IReadOnlyList<int>> CompleteQuestAsync(int questId, CancellationToken ct = default);

    Task AddRewardedEnergyAsync(int amount, CancellationToken ct = default);

    Task ApplyIapAsync(IapSku sku, CancellationToken ct = default);

    /// <summary>
    /// Level complete: oyuncu seviyesini 1 artirir. Biome unlock threshold'unu gecerse unlock tetikler.
    /// </summary>
    Task<LevelCompleteOutcome> OnLevelCompleteAsync(CancellationToken ct = default);

    /// <summary>Lifecycle: pause / low-memory durumunda senkron flush.</summary>
    Task FlushAsync(CancellationToken ct = default);

    /// <summary>Bir biyom icin unlock durumu. Player.CurrentLevel threshold'a gore.</summary>
    bool IsBiomeUnlocked(BiomeId biome);

    /// <summary>
    /// StarterPack teklifine kosul: L5+ ve (ilk gorulus pencere icinde) + daha once satin alinmamis.
    /// </summary>
    bool IsStarterPackOfferActive();
}

public readonly record struct MergeOutcome(
    bool Success,
    string? Reason,
    Item? MergedItem,
    int? RevealedFogTileIndex,
    int EnergyAfter,
    IReadOnlyList<int> QuestIdsProgressed);

public readonly record struct LevelCompleteOutcome(
    int NewLevel,
    BiomeId? UnlockedBiome);
