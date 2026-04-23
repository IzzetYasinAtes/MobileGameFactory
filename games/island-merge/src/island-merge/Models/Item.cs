using SQLite;

namespace IslandMerge.Models;

/// <summary>
/// Board'da yer alan tekil merge item. Pozisyon null ise envanterde.
/// </summary>
public class Item
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public ItemChain Chain { get; set; }

    /// <summary>1..5 arasi tier. Maks tier sabit (tavan).</summary>
    public int Tier { get; set; }

    /// <summary>Board'da bulunuyorsa 0..(Width*Height-1) hucre indeksi.</summary>
    public int? CellIndex { get; set; }

    public int PlayerId { get; set; }

    [Ignore]
    public string DisplayName => $"{Chain} T{Tier}";
}
