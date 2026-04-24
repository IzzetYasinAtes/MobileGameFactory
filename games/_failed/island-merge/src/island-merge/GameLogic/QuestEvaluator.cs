using IslandMerge.Models;

namespace IslandMerge.GameLogic;

/// <summary>
/// Quest ilerleme saf hesabi.
/// </summary>
public static class QuestEvaluator
{
    /// <summary>
    /// Uretilen (veya envantere eklenen) bir item quest hedefine uyuyorsa DeliveredQuantity arttirilir.
    /// True donerse quest state degisti demektir.
    /// </summary>
    public static bool TryContribute(Quest quest, Item item)
    {
        if (quest.Completed)
        {
            return false;
        }
        if (quest.TargetChain != item.Chain)
        {
            return false;
        }
        if (quest.TargetTier != item.Tier)
        {
            return false;
        }
        if (quest.DeliveredQuantity >= quest.TargetQuantity)
        {
            return false;
        }

        quest.DeliveredQuantity++;
        return true;
    }

    public static bool TryComplete(Quest quest)
    {
        if (!quest.CanComplete)
        {
            return false;
        }
        quest.Completed = true;
        return true;
    }
}
