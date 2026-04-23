namespace IslandMerge.Models;

/// <summary>
/// Biyom tanim tablosu. Level threshold'lari design.md'den gelir.
/// BiomeSelectViewModel + GameSession ortak kullanir.
/// </summary>
public static class BiomeCatalog
{
    public static readonly IReadOnlyList<BiomeDefinition> All = new List<BiomeDefinition>
    {
        new(BiomeId.TropicalForest, "Tropik Orman", "#2E8B57", 1,  20, true),
        new(BiomeId.Beach,           "Sahil / Magara",     "#1E90FF", 21, 40, false),
        new(BiomeId.Temple,          "Antik Tapinak",      "#B8860B", 41, 60, false),
        new(BiomeId.Volcano,         "Volkan",             "#CD5C5C", 61, 80, false),
        new(BiomeId.Ice,             "Buz Diyari",         "#9370DB", 81, 100, false),
    };

    public static BiomeDefinition Get(BiomeId id) =>
        All.FirstOrDefault(b => b.Id == id) ?? All[0];

    /// <summary>Bu biyoma karsilik gelen Resources/Images dosyasi.</summary>
    public static string ImageFor(BiomeId id) => id switch
    {
        BiomeId.TropicalForest => "env_forest.png",
        BiomeId.Beach => "env_sahil.png",
        BiomeId.Temple => "env_tapinak.png",
        BiomeId.Volcano => "env_volkan.png",
        BiomeId.Ice => "env_buz.png",
        _ => "env_forest.png",
    };
}
