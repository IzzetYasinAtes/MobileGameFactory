using SQLite;

namespace IslandMerge.Models;

public class FogTile
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int PlayerId { get; set; }

    public BiomeId Biome { get; set; }

    /// <summary>0..(FogWidth*FogHeight-1) tile indeksi.</summary>
    public int TileIndex { get; set; }

    public bool Revealed { get; set; }
}
