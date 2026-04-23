using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IslandMerge.Models;
using IslandMerge.Services;

namespace IslandMerge.ViewModels;

public sealed partial class BiomeSelectViewModel : BaseViewModel
{
    private readonly IGameSession _session;

    public ObservableCollection<BiomeCardVm> Biomes { get; } = new();

    public BiomeSelectViewModel(IGameSession session)
    {
        _session = session;
        Title = "Bolgeler";
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
            Biomes.Clear();
            foreach (var def in BiomeCatalog.All)
            {
                Biomes.Add(new BiomeCardVm
                {
                    Id = def.Id,
                    Name = def.Name,
                    AccentColorHex = def.AccentColorHex,
                    FirstLevel = def.FirstLevel,
                    LastLevel = def.LastLevel,
                    IsUnlocked = _session.IsBiomeUnlocked(def.Id),
                    ImageSource = BiomeCatalog.ImageFor(def.Id),
                });
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}

public partial class BiomeCardVm : ObservableObject
{
    [ObservableProperty]
    private BiomeId _id;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _accentColorHex = "#1F7A6C";

    [ObservableProperty]
    private int _firstLevel;

    [ObservableProperty]
    private int _lastLevel;

    [ObservableProperty]
    private bool _isUnlocked;

    [ObservableProperty]
    private string _imageSource = "env_forest.png";
}
