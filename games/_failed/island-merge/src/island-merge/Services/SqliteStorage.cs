using IslandMerge.Models;
using Microsoft.Extensions.Logging;
using SQLite;

namespace IslandMerge.Services;

public sealed partial class SqliteStorage : IStorage
{
    private readonly ILogger<SqliteStorage> _logger;
    private readonly string _dbPath;
    private SQLiteAsyncConnection? _db;

    // Test hook: dosya yolunu disaridan enjekte et.
    public SqliteStorage(ILogger<SqliteStorage> logger, string dbPath)
    {
        _logger = logger;
        _dbPath = dbPath;
    }

    public async Task InitializeAsync(CancellationToken ct = default)
    {
        if (_db is not null)
        {
            return;
        }

        try
        {
            await OpenAndMigrateAsync().ConfigureAwait(false);
            _logger.LogInformation("SQLite ready at {Path}", _dbPath);
        }
        catch (SQLiteException ex)
        {
            // P0-002: Corruption / locked-file recovery. Tek seferlik wipe + yeniden olustur.
            _logger.LogWarning(ex, "SQLite corruption detected at {Path}; recreating database", _dbPath);
            await RecreateDatabaseAsync().ConfigureAwait(false);
        }
        catch (IOException ex)
        {
            _logger.LogWarning(ex, "SQLite IO failure at {Path}; recreating database", _dbPath);
            await RecreateDatabaseAsync().ConfigureAwait(false);
        }
    }

    private async Task OpenAndMigrateAsync()
    {
        _db = new SQLiteAsyncConnection(
            _dbPath,
            SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);

        await _db.EnableWriteAheadLoggingAsync().ConfigureAwait(false);

        await _db.CreateTableAsync<Player>().ConfigureAwait(false);
        await _db.CreateTableAsync<Item>().ConfigureAwait(false);
        await _db.CreateTableAsync<Quest>().ConfigureAwait(false);
        await _db.CreateTableAsync<FogTile>().ConfigureAwait(false);
    }

    private async Task RecreateDatabaseAsync()
    {
        try
        {
            if (_db is not null)
            {
                await _db.CloseAsync().ConfigureAwait(false);
                _db = null;
            }
        }
        catch (Exception closeEx)
        {
            _logger.LogDebug(closeEx, "Ignoring close failure on recovery");
        }

        TryDeleteDbFile(_dbPath);
        TryDeleteDbFile(_dbPath + "-wal");
        TryDeleteDbFile(_dbPath + "-shm");
        TryDeleteDbFile(_dbPath + "-journal");

        await OpenAndMigrateAsync().ConfigureAwait(false);
        _logger.LogInformation("SQLite recovered (fresh schema) at {Path}", _dbPath);
    }

    private static void TryDeleteDbFile(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        catch
        {
            // Silent: recovery best-effort.
        }
    }

    private SQLiteAsyncConnection Db =>
        _db ?? throw new InvalidOperationException("Storage not initialized");

    public async Task<Player> GetOrCreatePlayerAsync(CancellationToken ct = default)
    {
        var existing = await Db.Table<Player>().FirstOrDefaultAsync().ConfigureAwait(false);
        if (existing is not null)
        {
            return existing;
        }

        var player = new Player
        {
            PetChoice = "Momo",
            Energy = 100,
            EnergyMax = 100,
            CurrentBiome = BiomeId.TropicalForest,
            CurrentLevel = 1,
            LastEnergyRefillUtc = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            LastLoginDateIso = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            SoftCurrency = 50,
            HardCurrency = 20,
        };

        await Db.InsertAsync(player).ConfigureAwait(false);
        return player;
    }

    public Task SavePlayerAsync(Player player, CancellationToken ct = default) =>
        Db.UpdateAsync(player);

    public async Task<IReadOnlyList<Item>> GetBoardItemsAsync(int playerId, CancellationToken ct = default)
    {
        var items = await Db.Table<Item>()
            .Where(i => i.PlayerId == playerId && i.CellIndex != null)
            .ToListAsync()
            .ConfigureAwait(false);
        return items;
    }

    public async Task UpsertItemAsync(Item item, CancellationToken ct = default)
    {
        if (item.Id == 0)
        {
            await Db.InsertAsync(item).ConfigureAwait(false);
        }
        else
        {
            await Db.UpdateAsync(item).ConfigureAwait(false);
        }
    }

    public Task DeleteItemAsync(int itemId, CancellationToken ct = default) =>
        Db.DeleteAsync<Item>(itemId);

    public async Task<IReadOnlyList<Quest>> GetActiveQuestsAsync(int playerId, BiomeId biome, int levelId, CancellationToken ct = default)
    {
        var list = await Db.Table<Quest>()
            .Where(q => q.PlayerId == playerId && q.Biome == biome && q.LevelId == levelId)
            .ToListAsync()
            .ConfigureAwait(false);
        return list;
    }

    public async Task UpsertQuestAsync(Quest quest, CancellationToken ct = default)
    {
        if (quest.Id == 0)
        {
            await Db.InsertAsync(quest).ConfigureAwait(false);
        }
        else
        {
            await Db.UpdateAsync(quest).ConfigureAwait(false);
        }
    }

    public async Task<IReadOnlyList<FogTile>> GetFogAsync(int playerId, BiomeId biome, CancellationToken ct = default)
    {
        var list = await Db.Table<FogTile>()
            .Where(f => f.PlayerId == playerId && f.Biome == biome)
            .ToListAsync()
            .ConfigureAwait(false);
        return list;
    }

    public async Task UpsertFogBatchAsync(IEnumerable<FogTile> tiles, CancellationToken ct = default)
    {
        foreach (var t in tiles)
        {
            if (t.Id == 0)
            {
                await Db.InsertAsync(t).ConfigureAwait(false);
            }
            else
            {
                await Db.UpdateAsync(t).ConfigureAwait(false);
            }
        }
    }

    public async Task SeedBiome1IfEmptyAsync(int playerId, CancellationToken ct = default)
    {
        var existing = await Db.Table<Item>()
            .Where(i => i.PlayerId == playerId)
            .CountAsync()
            .ConfigureAwait(false);

        if (existing > 0)
        {
            return;
        }

        // Seed: ilk ekranda 2 taş T1, 2 odun T1, 1 kristal T1, kalan bos.
        var seeds = new List<Item>
        {
            new() { PlayerId = playerId, Chain = ItemChain.Stone, Tier = 1, CellIndex = 0 },
            new() { PlayerId = playerId, Chain = ItemChain.Stone, Tier = 1, CellIndex = 1 },
            new() { PlayerId = playerId, Chain = ItemChain.Wood, Tier = 1, CellIndex = 7 },
            new() { PlayerId = playerId, Chain = ItemChain.Wood, Tier = 1, CellIndex = 8 },
            new() { PlayerId = playerId, Chain = ItemChain.Crystal, Tier = 1, CellIndex = 14 },
        };
        foreach (var s in seeds)
        {
            await Db.InsertAsync(s).ConfigureAwait(false);
        }

        // Seed biome1 quest L1: 2 tane T2 tas
        var quest = new Quest
        {
            PlayerId = playerId,
            Biome = BiomeId.TropicalForest,
            LevelId = 1,
            TargetChain = ItemChain.Stone,
            TargetTier = 2,
            TargetQuantity = 2,
            RewardXp = 20,
            RewardCoin = 30,
        };
        await Db.InsertAsync(quest).ConfigureAwait(false);

        // Seed fog 100 tile hepsi kapali.
        var tiles = new List<FogTile>();
        for (var i = 0; i < BoardConstants.FogTileCount; i++)
        {
            tiles.Add(new FogTile
            {
                PlayerId = playerId,
                Biome = BiomeId.TropicalForest,
                TileIndex = i,
                Revealed = false,
            });
        }
        foreach (var t in tiles)
        {
            await Db.InsertAsync(t).ConfigureAwait(false);
        }
    }
}
