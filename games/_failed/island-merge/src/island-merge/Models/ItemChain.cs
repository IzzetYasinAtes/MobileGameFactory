namespace IslandMerge.Models;

/// <summary>
/// Farkli kaynak zincirleri. Design.md: 3 temel kaynak x 5 tier.
/// Biyom-spesifik ek zincirler gelecek (Sahil: Kabuk; Tapinak: Madalyon...).
/// </summary>
public enum ItemChain
{
    None = 0,
    Stone = 1,
    Wood = 2,
    Crystal = 3,
    // Biome-specific chains (seed ileride; simdi sadece enum slot)
    Shell = 10,
    Relic = 11,
    Ember = 12,
    Ice = 13,
}
