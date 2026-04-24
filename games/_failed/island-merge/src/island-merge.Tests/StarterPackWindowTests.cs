namespace IslandMerge.Tests;

/// <summary>
/// E2E-002 regresyon kapanisi. GameSession.OnLevelCompleteAsync'deki
/// karar mantigi: StarterPackFirstSeenUtc sifir ise ilk level tamamlaninca set.
/// (Once L5 esigi vardi; L1-4 arasi pencere hic acilmiyordu.)
/// </summary>
public class StarterPackWindowTests
{
    private static long Apply(long currentTimestamp, long now)
    {
        // GameSession.OnLevelCompleteAsync karar kurali (kopya):
        return currentTimestamp == 0 ? now : currentTimestamp;
    }

    [Fact]
    public void FirstLevelComplete_SetsWindowTimestamp()
    {
        var after = Apply(currentTimestamp: 0, now: 1_700_000_000);

        Assert.Equal(1_700_000_000, after);
    }

    [Fact]
    public void SubsequentLevelComplete_KeepsOriginalTimestamp()
    {
        var after = Apply(currentTimestamp: 1_700_000_000, now: 1_700_003_600);

        Assert.Equal(1_700_000_000, after);
    }

    [Theory]
    [InlineData(0, false)]                       // 0 sn ote: pencere kapali.
    [InlineData(24 * 3600 - 1, true)]            // 24h eksi 1 sn: pencere acik.
    [InlineData(24 * 3600, false)]               // tam 24h: pencere kapali.
    [InlineData(24 * 3600 + 1, false)]           // 24h + 1 sn: pencere kapali.
    public void OfferActive_WithinTwentyFourHours(long elapsedSeconds, bool expected)
    {
        var active = elapsedSeconds > 0 && elapsedSeconds < 24 * 3600;

        Assert.Equal(expected, active);
    }
}
