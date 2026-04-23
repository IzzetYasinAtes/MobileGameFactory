using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IslandMerge.Models;
using IslandMerge.Services;

namespace IslandMerge.ViewModels;

public sealed partial class MainMenuViewModel : BaseViewModel
{
    private readonly IGameSession _session;
    private readonly ISelectedCharacterStore _characters;

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

    public ObservableCollection<CharacterCardVm> CharacterCarousel { get; } = new();

    public MainMenuViewModel(IGameSession session, ISelectedCharacterStore characters)
    {
        _session = session;
        _characters = characters;
        Title = "Mini Kasifler";
        SeedCarousel();
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
            BiomeName = BiomeCatalog.Get(p.CurrentBiome).Name;
            SyncCarouselSelection();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task PlayAsync()
    {
        // Ilk acilistaki karakter secimini zorlama yerine burada yumusak yonlendirme.
        if (!_characters.HasSelection)
        {
            await Shell.Current.GoToAsync("character-select").ConfigureAwait(true);
            return;
        }
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

    [RelayCommand]
    public async Task OpenCharacterSelectAsync()
    {
        await Shell.Current.GoToAsync("character-select").ConfigureAwait(true);
    }

    private void SeedCarousel()
    {
        CharacterCarousel.Clear();
        CharacterCarousel.Add(new CharacterCardVm { Id = CharacterId.Kasif, Name = "Kasif", ImageSource = "character_kasif.png" });
        CharacterCarousel.Add(new CharacterCardVm { Id = CharacterId.Lila, Name = "Lila", ImageSource = "character_lila.png" });
        CharacterCarousel.Add(new CharacterCardVm { Id = CharacterId.Momo, Name = "Momo", ImageSource = "character_momo.png" });
        CharacterCarousel.Add(new CharacterCardVm { Id = CharacterId.Papagan, Name = "Papagan", ImageSource = "character_papagan.png" });
        SyncCarouselSelection();
    }

    private void SyncCarouselSelection()
    {
        var active = _characters.GetSelected();
        foreach (var c in CharacterCarousel)
        {
            c.IsSelected = c.Id == active;
        }
    }
}
