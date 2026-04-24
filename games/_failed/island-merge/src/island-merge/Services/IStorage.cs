using IslandMerge.Models;

namespace IslandMerge.Services;

public interface IStorage
{
    Task InitializeAsync(CancellationToken ct = default);

    Task<Player> GetOrCreatePlayerAsync(CancellationToken ct = default);

    Task SavePlayerAsync(Player player, CancellationToken ct = default);

    Task<IReadOnlyList<Item>> GetBoardItemsAsync(int playerId, CancellationToken ct = default);

    Task UpsertItemAsync(Item item, CancellationToken ct = default);

    Task DeleteItemAsync(int itemId, CancellationToken ct = default);

    Task<IReadOnlyList<Quest>> GetActiveQuestsAsync(int playerId, BiomeId biome, int levelId, CancellationToken ct = default);

    Task UpsertQuestAsync(Quest quest, CancellationToken ct = default);

    Task<IReadOnlyList<FogTile>> GetFogAsync(int playerId, BiomeId biome, CancellationToken ct = default);

    Task UpsertFogBatchAsync(IEnumerable<FogTile> tiles, CancellationToken ct = default);

    Task SeedBiome1IfEmptyAsync(int playerId, CancellationToken ct = default);
}
