using IslandMerge.Models;

namespace IslandMerge.GameLogic;

/// <summary>
/// Saf C# merge mantigi. Unit test'in ana hedefi.
/// Kural: ayni chain + ayni tier -> tier+1 item.
/// MaxTier tavani asilmaz; tavandaki 2 item merge edilmez (null doner).
/// Farkli chain veya tier merge edilmez.
/// </summary>
public static class MergeEngine
{
    public static MergeResult TryMerge(Item a, Item b)
    {
        if (a is null || b is null)
        {
            return MergeResult.Fail("null item");
        }

        if (a.Id != 0 && a.Id == b.Id)
        {
            return MergeResult.Fail("same instance");
        }

        if (a.Chain == ItemChain.None || b.Chain == ItemChain.None)
        {
            return MergeResult.Fail("unchained");
        }

        if (a.Chain != b.Chain)
        {
            return MergeResult.Fail("chain mismatch");
        }

        if (a.Tier != b.Tier)
        {
            return MergeResult.Fail("tier mismatch");
        }

        if (a.Tier >= BoardConstants.MaxTier)
        {
            return MergeResult.Fail("max tier");
        }

        var merged = new Item
        {
            Chain = a.Chain,
            Tier = a.Tier + 1,
            PlayerId = a.PlayerId,
            CellIndex = a.CellIndex,
        };

        return MergeResult.Ok(merged);
    }

    /// <summary>
    /// Tier'a bagli enerji maliyeti. Design.md: 1 + floor(tier/2).
    /// T1-T2 = 1, T3-T4 = 2, T5 = 3.
    /// </summary>
    public static int EnergyCostForMerge(int tier)
    {
        if (tier < 1)
        {
            tier = 1;
        }

        return 1 + (tier / 2);
    }
}

public readonly record struct MergeResult(bool Success, Item? Result, string? Reason)
{
    public static MergeResult Ok(Item item) => new(true, item, null);
    public static MergeResult Fail(string reason) => new(false, null, reason);
}
