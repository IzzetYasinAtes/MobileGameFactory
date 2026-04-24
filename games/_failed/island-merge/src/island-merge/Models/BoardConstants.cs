namespace IslandMerge.Models;

public static class BoardConstants
{
    public const int Width = 7;
    public const int Height = 9;
    public const int CellCount = Width * Height;

    public const int FogWidth = 10;
    public const int FogHeight = 10;
    public const int FogTileCount = FogWidth * FogHeight;

    public const int MaxTier = 5;

    /// <summary>Design.md: her 3 merge bir fog tile acar.</summary>
    public const int MergesPerFogReveal = 3;

    /// <summary>Design.md: quest tamamlanmasi +3 fog tile.</summary>
    public const int QuestBonusFogReveal = 3;

    /// <summary>Energy cooldown saniye per point (L16+).</summary>
    public const int EnergyCooldownSeconds = 90;

    /// <summary>Energy bariyer esigi.</summary>
    public const int EnergyBarrierLevel = 16;
}
