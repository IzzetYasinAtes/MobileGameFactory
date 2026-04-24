namespace IslandMerge.GameLogic;

/// <summary>
/// Design.md formulleri:
/// MergeHedefi(L) = 3 + floor(L/5)
/// SureToleransi(L) = max(90, 180 - L*0.8)
/// </summary>
public static class DifficultyCurve
{
    public static int MergeGoal(int level)
    {
        if (level < 1)
        {
            level = 1;
        }
        return 3 + (level / 5);
    }

    public static int TimeToleranceSeconds(int level)
    {
        if (level < 1)
        {
            level = 1;
        }
        var s = 180 - (int)(level * 0.8);
        return Math.Max(90, s);
    }

    public static int XpRewardForLevel(int level)
    {
        return 10 + (level * 2);
    }

    public static int CoinRewardForLevel(int level)
    {
        return 20 + (level * 3);
    }
}
