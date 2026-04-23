using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IslandMerge.Models;
using IslandMerge.Services;

namespace IslandMerge.ViewModels;

public sealed partial class BiomeSelectViewModel : BaseViewModel
{
    private const string SeenUnlockedKey = "biome_seen_unlocked";

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

            var seenCsv = Preferences.Default.Get(SeenUnlockedKey, string.Empty);
            var seen = new HashSet<string>(
                seenCsv.Split(',', StringSplitOptions.RemoveEmptyEntries));

            Biomes.Clear();
            var newlyUnlocked = new List<string>();
            foreach (var def in BiomeCatalog.All)
            {
                var unlocked = _session.IsBiomeUnlocked(def.Id);
                var idKey = ((int)def.Id).ToString();
                var justUnlocked = unlocked && !seen.Contains(idKey);
                if (justUnlocked)
                {
                    newlyUnlocked.Add(idKey);
                }

                Biomes.Add(new BiomeCardVm
                {
                    Id = def.Id,
                    Name = def.Name,
                    AccentColorHex = def.AccentColorHex,
                    FirstLevel = def.FirstLevel,
                    LastLevel = def.LastLevel,
                    IsUnlocked = unlocked,
                    ImageSource = BiomeCatalog.ImageFor(def.Id),
                    JustUnlocked = justUnlocked,
                });
            }

            // Page animation oynattiktan sonra kalicilastir: bir daha reveal calmasin.
            if (newlyUnlocked.Count > 0)
            {
                foreach (var k in newlyUnlocked)
                {
                    seen.Add(k);
                }
                Preferences.Default.Set(SeenUnlockedKey, string.Join(',', seen));
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

    /// <summary>
    /// Animator bayragi: bu yukleme sirasinda ilk kez acilan biome.
    /// Page code-behind okur, UnlockReveal animation oynatir.
    /// </summary>
    public bool JustUnlocked { get; set; }
}
