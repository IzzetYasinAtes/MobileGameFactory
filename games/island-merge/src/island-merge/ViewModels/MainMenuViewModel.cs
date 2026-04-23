using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IslandMerge.Services;

namespace IslandMerge.ViewModels;

public sealed partial class MainMenuViewModel : BaseViewModel
{
    private readonly IGameSession _session;

    [ObservableProperty]
    private int _currentLevel;

    [ObservableProperty]
    private string _biomeName = "Tropik Orman";

    [ObservableProperty]
    private int _softCurrency;

    [ObservableProperty]
    private int _hardCurrency;

    [ObservableProperty]
    private int _energy;

    public MainMenuViewModel(IGameSession session)
    {
        _session = session;
        Title = "Mini Kasifler";
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
            var p = _session.Player;
            CurrentLevel = p.CurrentLevel;
            SoftCurrency = p.SoftCurrency;
            HardCurrency = p.HardCurrency;
            Energy = p.Energy;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task PlayAsync()
    {
        await Shell.Current.GoToAsync("board").ConfigureAwait(true);
    }

    [RelayCommand]
    public async Task OpenBiomeSelectAsync()
    {
        await Shell.Current.GoToAsync("biome-select").ConfigureAwait(true);
    }

    [RelayCommand]
    public async Task OpenShopAsync()
    {
        await Shell.Current.GoToAsync("shop").ConfigureAwait(true);
    }

    [RelayCommand]
    public async Task OpenSettingsAsync()
    {
        await Shell.Current.GoToAsync("settings").ConfigureAwait(true);
    }
}
