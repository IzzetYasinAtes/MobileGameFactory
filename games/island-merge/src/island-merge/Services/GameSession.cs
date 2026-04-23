using IslandMerge.GameLogic;
using IslandMerge.Models;
using Microsoft.Extensions.Logging;

namespace IslandMerge.Services;

public sealed class GameSession : IGameSession
{
    private readonly IStorage _storage;
    private readonly IAudio _audio;
    private readonly ILogger<GameSession> _logger;
    private readonly List<Item> _boardItems = new();
    private readonly List<Quest> _activeQuests = new();
    private FogSystem _fog = new(BoardConstants.FogTileCount);

    private Player? _player;

    public GameSession(IStorage storage, IAudio audio, ILogger<GameSession> logger)
    {
        _storage = storage;
        _audio = audio;
        _logger = logger;
    }

    public Player Player => _player ?? throw new InvalidOperationException("Session not loaded");

    public IReadOnlyList<Item> BoardItems => _boardItems;

    public IReadOnlyList<Quest> ActiveQuests => _activeQuests;

    public IReadOnlyList<bool> FogMask => _fog.Snapshot();

    public int LastRevealedTileIndex { get; private set; } = -1;

    public async Task LoadAsync(CancellationToken ct = default)
    {
        await _storage.InitializeAsync(ct).ConfigureAwait(false);
        _player = await _storage.GetOrCreatePlayerAsync(ct).ConfigureAwait(false);
        await _storage.SeedBiome1IfEmptyAsync(_player.Id, ct).ConfigureAwait(false);

        var items = await _storage.GetBoardItemsAsync(_player.Id, ct).ConfigureAwait(false);
        _boardItems.Clear();
        _boardItems.AddRange(items);

        var quests = await _storage.GetActiveQuestsAsync(_player.Id, _player.CurrentBiome, _player.CurrentLevel, ct)
            .ConfigureAwait(false);
        _activeQuests.Clear();
        _activeQuests.AddRange(quests);

        var fog = await _storage.GetFogAsync(_player.Id, _player.CurrentBiome, ct).ConfigureAwait(false);
        var mask = new bool[BoardConstants.FogTileCount];
        foreach (var t in fog)
        {
            if (t.TileIndex >= 0 && t.TileIndex < mask.Length)
            {
                mask[t.TileIndex] = t.Revealed;
            }
        }
        _fog = new FogSystem(BoardConstants.FogTileCount);
        _fog.LoadFromMask(mask);

        // Pasif enerji rejenerasyonu.
        EnergySystem.Regenerate(_player, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        await _storage.SavePlayerAsync(_player, ct).ConfigureAwait(false);
    }

    public async Task<MergeOutcome> TryMergeAsync(int sourceCellIndex, int targetCellIndex, CancellationToken ct = default)
    {
        if (_player is null)
        {
            return new MergeOutcome(false, "not loaded", null, null, 0, Array.Empty<int>());
        }

        if (sourceCellIndex == targetCellIndex)
        {
            return new MergeOutcome(false, "same cell", null, null, _player.Energy, Array.Empty<int>());
        }

        var source = _boardItems.FirstOrDefault(i => i.CellIndex == sourceCellIndex);
        var target = _boardItems.FirstOrDefault(i => i.CellIndex == targetCellIndex);
        if (source is null || target is null)
        {
            return new MergeOutcome(false, "empty cell", null, null, _player.Energy, Array.Empty<int>());
        }

        var attempt = MergeEngine.TryMerge(source, target);
        if (!attempt.Success || attempt.Result is null)
        {
            _audio.PlaySfx(SfxKind.Error);
            return new MergeOutcome(false, attempt.Reason, null, null, _player.Energy, Array.Empty<int>());
        }

        var cost = MergeEngine.EnergyCostForMerge(source.Tier);
        if (EnergySystem.HasCooldown(_player.CurrentLevel) && _player.Energy < cost)
        {
            return new MergeOutcome(false, "low energy", null, null, _player.Energy, Array.Empty<int>());
        }

        EnergySystem.Consume(_player, cost, DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        // Merge uygulandi: source ve target sil, yeni item yeni hucreye yerlestir (hedef hucre).
        _boardItems.Remove(source);
        _boardItems.Remove(target);

        if (source.Id != 0)
        {
            await _storage.DeleteItemAsync(source.Id, ct).ConfigureAwait(false);
        }
        if (target.Id != 0)
        {
            await _storage.DeleteItemAsync(target.Id, ct).ConfigureAwait(false);
        }

        var merged = attempt.Result;
        merged.PlayerId = _player.Id;
        merged.CellIndex = targetCellIndex;
        await _storage.UpsertItemAsync(merged, ct).ConfigureAwait(false);
        _boardItems.Add(merged);

        _audio.PlaySfx(SfxKind.MergePop);

        // Quest katkisi.
        var progressed = new List<int>();
        foreach (var q in _activeQuests)
        {
            if (QuestEvaluator.TryContribute(q, merged))
            {
                await _storage.UpsertQuestAsync(q, ct).ConfigureAwait(false);
                progressed.Add(q.Id);
            }
        }

        // Fog reveal.
        var revealed = _fog.RegisterMergeAndMaybeReveal();
        if (revealed is int tileIndex)
        {
            LastRevealedTileIndex = tileIndex;
            _audio.PlaySfx(SfxKind.FogReveal);
            await _storage.UpsertFogBatchAsync(new[]
            {
                new FogTile
                {
                    PlayerId = _player.Id,
                    Biome = _player.CurrentBiome,
                    TileIndex = tileIndex,
                    Revealed = true,
                },
            }, ct).ConfigureAwait(false);
        }

        await _storage.SavePlayerAsync(_player, ct).ConfigureAwait(false);

        return new MergeOutcome(true, null, merged, revealed, _player.Energy, progressed);
    }

    public async Task<IReadOnlyList<int>> CompleteQuestAsync(int questId, CancellationToken ct = default)
    {
        if (_player is null)
        {
            return Array.Empty<int>();
        }

        var quest = _activeQuests.FirstOrDefault(q => q.Id == questId);
        if (quest is null)
        {
            return Array.Empty<int>();
        }

        if (!QuestEvaluator.TryComplete(quest))
        {
            return Array.Empty<int>();
        }

        _player.TotalXp += quest.RewardXp;
        _player.SoftCurrency += quest.RewardCoin;
        _audio.PlaySfx(SfxKind.QuestComplete);

        await _storage.UpsertQuestAsync(quest, ct).ConfigureAwait(false);

        var bonusTiles = _fog.RevealBatch(BoardConstants.QuestBonusFogReveal);
        if (bonusTiles.Count > 0)
        {
            LastRevealedTileIndex = bonusTiles[^1];
            await _storage.UpsertFogBatchAsync(
                bonusTiles.Select(i => new FogTile
                {
                    PlayerId = _player.Id,
                    Biome = _player.CurrentBiome,
                    TileIndex = i,
                    Revealed = true,
                }), ct).ConfigureAwait(false);
        }

        await _storage.SavePlayerAsync(_player, ct).ConfigureAwait(false);
        return bonusTiles;
    }

    public async Task AddRewardedEnergyAsync(int amount, CancellationToken ct = default)
    {
        if (_player is null || amount <= 0)
        {
            return;
        }
        EnergySystem.Grant(_player, amount);
        await _storage.SavePlayerAsync(_player, ct).ConfigureAwait(false);
    }

    public async Task ApplyIapAsync(IapSku sku, CancellationToken ct = default)
    {
        if (_player is null)
        {
            return;
        }

        switch (sku)
        {
            case IapSku.Energy100:
                EnergySystem.Grant(_player, 100);
                break;
            case IapSku.Energy500:
                EnergySystem.GrantOvercap(_player, 500);
                break;
            case IapSku.StarterPack:
                EnergySystem.GrantOvercap(_player, 500);
                _player.HardCurrency += 200;
                _player.StarterPackPurchased = true;
                break;
            case IapSku.RemoveAds:
                _player.RemoveAdsPurchased = true;
                break;
        }

        await _storage.SavePlayerAsync(_player, ct).ConfigureAwait(false);
    }

    public async Task<LevelCompleteOutcome> OnLevelCompleteAsync(CancellationToken ct = default)
    {
        if (_player is null)
        {
            return new LevelCompleteOutcome(0, null);
        }

        var previousLevel = _player.CurrentLevel;
        _player.CurrentLevel = previousLevel + 1;

        // Biome unlock tetikleme. Her biyomun FirstLevel esigi gecildiyse unlock.
        BiomeId? unlocked = null;
        foreach (var def in BiomeCatalog.All)
        {
            var justUnlocked = previousLevel < def.FirstLevel && _player.CurrentLevel >= def.FirstLevel;
            if (justUnlocked && def.Id != _player.CurrentBiome)
            {
                unlocked = def.Id;
                _logger.LogInformation("Biome {Biome} unlocked at level {Level}", def.Id, _player.CurrentLevel);
                break;
            }
        }

        // StarterPack penceresi: ilk level tamamlandiginda 24h sayaci baslat (design: "launch sonrasi 24h").
        // L5 esigi E2E-002'de L1-4 arasi oyuncunun teklifi hic gormemesine yol aciyordu; ilk run
        // bitiminde pencere acilir, 24h sonra kapanir.
        if (_player.StarterPackFirstSeenUtc == 0)
        {
            _player.StarterPackFirstSeenUtc = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        await _storage.SavePlayerAsync(_player, ct).ConfigureAwait(false);
        return new LevelCompleteOutcome(_player.CurrentLevel, unlocked);
    }

    public async Task FlushAsync(CancellationToken ct = default)
    {
        if (_player is null)
        {
            return;
        }

        try
        {
            await _storage.SavePlayerAsync(_player, ct).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Flush failed");
        }
    }

    public bool IsBiomeUnlocked(BiomeId biome)
    {
        if (_player is null)
        {
            return biome == BiomeId.TropicalForest;
        }
        var def = BiomeCatalog.Get(biome);
        return _player.CurrentLevel >= def.FirstLevel;
    }

    public bool IsStarterPackOfferActive()
    {
        if (_player is null)
        {
            return false;
        }
        if (_player.StarterPackPurchased)
        {
            return false;
        }
        if (_player.StarterPackFirstSeenUtc == 0)
        {
            return false;
        }
        var elapsedSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - _player.StarterPackFirstSeenUtc;
        return elapsedSeconds < 24 * 3600;
    }
}
