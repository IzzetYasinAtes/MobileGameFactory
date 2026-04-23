using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IslandMerge.Models;
using IslandMerge.Services;
using Microsoft.Extensions.Logging;

namespace IslandMerge.ViewModels;

public sealed partial class BoardViewModel : BaseViewModel
{
    private readonly IGameSession _session;
    private readonly IAdService _adService;
    private readonly IInterstitialGuard _interstitialGuard;
    private readonly IRewardedCooldown _rewardedCooldown;
    private readonly ILogger<BoardViewModel> _logger;

    [ObservableProperty]
    private int _energy;

    [ObservableProperty]
    private string _questText = string.Empty;

    [ObservableProperty]
    private string _statusText = string.Empty;

    [ObservableProperty]
    private int _revealedCount;

    [ObservableProperty]
    private bool _canWatchAdForEnergy = true;

    public ObservableCollection<BoardCellVm> Cells { get; } = new();

    public BoardViewModel(
        IGameSession session,
        IAdService adService,
        IInterstitialGuard interstitialGuard,
        IRewardedCooldown rewardedCooldown,
        ILogger<BoardViewModel> logger)
    {
        _session = session;
        _adService = adService;
        _interstitialGuard = interstitialGuard;
        _rewardedCooldown = rewardedCooldown;
        _logger = logger;
        Title = "Ada";
        for (var i = 0; i < BoardConstants.CellCount; i++)
        {
            Cells.Add(new BoardCellVm { Index = i });
        }
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy)
        {
            return;
        }
        IsBusy = true;
        try
        {
            await _session.LoadAsync().ConfigureAwait(true);
            _interstitialGuard.NotifyRunStarted();
            RebuildCells();
            UpdateHud();
            RefreshAdAvailability();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task TryMergeAsync((int src, int dst) move)
    {
        var outcome = await _session.TryMergeAsync(move.src, move.dst).ConfigureAwait(true);
        if (!outcome.Success)
        {
            StatusText = outcome.Reason ?? "Birlestirilemedi";
            return;
        }

        StatusText = $"{outcome.MergedItem!.Chain} T{outcome.MergedItem.Tier}";
        RebuildCells();
        UpdateHud();

        foreach (var qid in outcome.QuestIdsProgressed)
        {
            var q = _session.ActiveQuests.FirstOrDefault(x => x.Id == qid);
            if (q is not null && q.CanComplete)
            {
                await _session.CompleteQuestAsync(q.Id).ConfigureAwait(true);
                StatusText = "Gorev tamam!";
                UpdateHud();
                await OnLevelCompleteAsync().ConfigureAwait(true);
            }
        }
    }

    [RelayCommand]
    public async Task WatchAdForEnergyAsync()
    {
        if (!_rewardedCooldown.IsReady(AdPlacement.EnergyRefill))
        {
            var left = _rewardedCooldown.TimeLeft(AdPlacement.EnergyRefill);
            StatusText = left is { } t
                ? $"Tekrar izlemek icin {Math.Max(1, (int)t.TotalSeconds)} sn"
                : "Kisa sure bekleyin";
            return;
        }

        var result = await _adService.ShowRewardedAsync(AdPlacement.EnergyRefill).ConfigureAwait(true);
        if (result.Rewarded)
        {
            _rewardedCooldown.NotifyShown(AdPlacement.EnergyRefill);
            await _session.AddRewardedEnergyAsync(50).ConfigureAwait(true);
            UpdateHud();
            RefreshAdAvailability();
            StatusText = "+50 Enerji";
        }
        else
        {
            StatusText = "Reklam yuklenemedi";
        }
    }

    private async Task OnLevelCompleteAsync()
    {
        var outcome = await _session.OnLevelCompleteAsync().ConfigureAwait(true);
        if (outcome.UnlockedBiome is { } biome)
        {
            StatusText = $"Yeni bolge: {BiomeCatalog.Get(biome).Name}!";
        }

        // Interstitial guard: kurallari uygular, silently skip.
        await _interstitialGuard
            .TryShowOnLevelCompleteAsync(_session.Player.CurrentLevel, _session.Player.RemoveAdsPurchased)
            .ConfigureAwait(true);
    }

    private void RefreshAdAvailability()
    {
        CanWatchAdForEnergy = _rewardedCooldown.IsReady(AdPlacement.EnergyRefill);
    }

    private void RebuildCells()
    {
        foreach (var c in Cells)
        {
            c.Chain = ItemChain.None;
            c.Tier = 0;
        }
        foreach (var i in _session.BoardItems)
        {
            if (i.CellIndex is int idx && idx >= 0 && idx < Cells.Count)
            {
                Cells[idx].Chain = i.Chain;
                Cells[idx].Tier = i.Tier;
            }
        }
    }

    private void UpdateHud()
    {
        Energy = _session.Player.Energy;
        var q = _session.ActiveQuests.FirstOrDefault(x => !x.Completed);
        QuestText = q is null
            ? "Tum gorevler tamam"
            : $"Hedef: {q.TargetQuantity}x {q.TargetChain} T{q.TargetTier} ({q.DeliveredQuantity}/{q.TargetQuantity})";
        var revealed = 0;
        foreach (var r in _session.FogMask)
        {
            if (r)
            {
                revealed++;
            }
        }
        RevealedCount = revealed;
    }
}

public partial class BoardCellVm : ObservableObject
{
    [ObservableProperty]
    private int _index;

    [ObservableProperty]
    private ItemChain _chain;

    [ObservableProperty]
    private int _tier;
}
