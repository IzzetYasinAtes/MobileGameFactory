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
    private readonly ILogger<BoardViewModel> _logger;

    [ObservableProperty]
    private int _energy;

    [ObservableProperty]
    private string _questText = string.Empty;

    [ObservableProperty]
    private string _statusText = string.Empty;

    [ObservableProperty]
    private int _revealedCount;

    public ObservableCollection<BoardCellVm> Cells { get; } = new();

    public BoardViewModel(IGameSession session, IAdService adService, ILogger<BoardViewModel> logger)
    {
        _session = session;
        _adService = adService;
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
            RebuildCells();
            UpdateHud();
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
            }
        }
    }

    [RelayCommand]
    public async Task WatchAdForEnergyAsync()
    {
        var result = await _adService.ShowRewardedAsync(AdPlacement.EnergyRefill).ConfigureAwait(true);
        if (result.Rewarded)
        {
            await _session.AddRewardedEnergyAsync(50).ConfigureAwait(true);
            UpdateHud();
            StatusText = "+50 Enerji";
        }
        else
        {
            StatusText = "Reklam yuklenemedi";
        }
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
