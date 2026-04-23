namespace IslandMerge.Models;

public enum BiomeId
{
    TropicalForest = 1,
    Beach = 2,
    Temple = 3,
    Volcano = 4,
    Ice = 5,
}

public record BiomeDefinition(
    BiomeId Id,
    string Name,
    string AccentColorHex,
    int FirstLevel,
    int LastLevel,
    bool Unlocked);
