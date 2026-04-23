using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using IslandMerge.Models;

namespace IslandMerge.ViewModels;

public sealed partial class BiomeSelectViewModel : BaseViewModel
{
    public ObservableCollection<BiomeDefinition> Biomes { get; } = new();

    public BiomeSelectViewModel()
    {
        Title = "Bolgeler";
        Biomes.Add(new BiomeDefinition(BiomeId.TropicalForest, "Tropik Orman", "#2E8B57", 1, 20, true));
        Biomes.Add(new BiomeDefinition(BiomeId.Beach, "Sahil / Magara", "#1E90FF", 21, 40, false));
        Biomes.Add(new BiomeDefinition(BiomeId.Temple, "Antik Tapinak", "#B8860B", 41, 60, false));
        Biomes.Add(new BiomeDefinition(BiomeId.Volcano, "Volkan", "#CD5C5C", 61, 80, false));
        Biomes.Add(new BiomeDefinition(BiomeId.Ice, "Buz Diyari", "#9370DB", 81, 100, false));
    }
}
