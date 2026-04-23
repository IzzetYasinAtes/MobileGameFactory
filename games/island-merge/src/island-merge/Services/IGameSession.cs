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
}

public readonly record struct MergeOutcome(
    bool Success,
    string? Reason,
    Item? MergedItem,
    int? RevealedFogTileIndex,
    int EnergyAfter,
    IReadOnlyList<int> QuestIdsProgressed);
