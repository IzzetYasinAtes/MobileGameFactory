using IslandMerge.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace IslandMerge.Tests;

public sealed class InterstitialGuardTests
{
    private sealed class CountingAdService : IAdService
    {
        public int InterstitialShowCount { get; private set; }

        public bool IsInitialized => true;

        public Task InitializeAsync(CancellationToken ct = default) => Task.CompletedTask;

        public Task<AdResult> ShowRewardedAsync(AdPlacement placement, CancellationToken ct = default) =>
            Task.FromResult(new AdResult(true, true, null));

        public Task<bool> ShowInterstitialAsync(CancellationToken ct = default)
        {
            InterstitialShowCount++;
            return Task.FromResult(true);
        }
    }

    private static InterstitialGuard Build(CountingAdService ads, out Func<DateTimeOffset> clock, DateTimeOffset? start = null)
    {
        var t = start ?? new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        DateTimeOffset current = t;
        clock = () => current;

        var guard = new InterstitialGuard(ads, NullLogger<InterstitialGuard>.Instance, () => current);
        return guard;
    }

    [Fact]
    public async Task Show_FirstThreeRuns_NeverShows()
    {
        var ads = new CountingAdService();
        var guard = Build(ads, out _);
        guard.NotifySessionStarted();

        for (var run = 1; run <= 3; run++)
        {
            guard.NotifyRunStarted();
            var shown = await guard.TryShowOnLevelCompleteAsync(currentLevel: 10, removeAdsPurchased: false);
            Assert.False(shown, $"run {run} should be suppressed");
        }

        Assert.Equal(0, ads.InterstitialShowCount);
    }

    [Fact]
    public async Task Show_LevelBelowMin_DoesNotShow()
    {
        var ads = new CountingAdService();
        var guard = Build(ads, out _);
        guard.NotifySessionStarted();

        // Run count over threshold.
        for (var r = 0; r < 5; r++)
        {
            guard.NotifyRunStarted();
        }

        var shown = await guard.TryShowOnLevelCompleteAsync(currentLevel: 3, removeAdsPurchased: false);
        Assert.False(shown);
        Assert.Equal(0, ads.InterstitialShowCount);
    }

    [Fact]
    public async Task Show_RemoveAdsPurchased_AlwaysSuppressed()
    {
        var ads = new CountingAdService();
        var guard = Build(ads, out _);
        guard.NotifySessionStarted();
        for (var r = 0; r < 10; r++)
        {
            guard.NotifyRunStarted();
        }

        var shown = await guard.TryShowOnLevelCompleteAsync(currentLevel: 20, removeAdsPurchased: true);
        Assert.False(shown);
        Assert.Equal(0, ads.InterstitialShowCount);
    }

    [Fact]
    public async Task Show_AfterFourRuns_AtLevel4_Shows()
    {
        var ads = new CountingAdService();
        var guard = Build(ads, out _);
        guard.NotifySessionStarted();

        // 4 runs: ilk 3 yasak, 4. run'da asinmasi gerek (ama sadece level >= 4 ise).
        for (var r = 0; r < 4; r++)
        {
            guard.NotifyRunStarted();
        }

        var shown = await guard.TryShowOnLevelCompleteAsync(currentLevel: InterstitialGuard.MinLevel, removeAdsPurchased: false);
        Assert.True(shown);
        Assert.Equal(1, ads.InterstitialShowCount);
    }

    [Fact]
    public async Task Show_SessionCap_BlocksThirdAttempt()
    {
        var ads = new CountingAdService();
        DateTimeOffset now = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var guard = new InterstitialGuard(ads, NullLogger<InterstitialGuard>.Instance, () => now);
        guard.NotifySessionStarted();
        for (var r = 0; r < 10; r++)
        {
            guard.NotifyRunStarted();
        }

        // 1. gosterim
        Assert.True(await guard.TryShowOnLevelCompleteAsync(10, false));
        // Min interval gectikten sonra 2. gosterim
        now = now.AddMinutes(5);
        Assert.True(await guard.TryShowOnLevelCompleteAsync(10, false));
        // Cap = 2. 3. deneme reddedilmeli.
        now = now.AddMinutes(5);
        Assert.False(await guard.TryShowOnLevelCompleteAsync(10, false));

        Assert.Equal(2, ads.InterstitialShowCount);
    }

    [Fact]
    public async Task Show_MinInterval_NotMet_Suppressed()
    {
        var ads = new CountingAdService();
        DateTimeOffset now = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var guard = new InterstitialGuard(ads, NullLogger<InterstitialGuard>.Instance, () => now);
        guard.NotifySessionStarted();
        for (var r = 0; r < 10; r++)
        {
            guard.NotifyRunStarted();
        }

        Assert.True(await guard.TryShowOnLevelCompleteAsync(10, false));

        // 2 dakika sonra: 4 dk interval dolmamis, reddedilmeli.
        now = now.AddMinutes(2);
        Assert.False(await guard.TryShowOnLevelCompleteAsync(10, false));

        // 3 dakika sonra (toplam 5 dk): interval gecti.
        now = now.AddMinutes(3);
        Assert.True(await guard.TryShowOnLevelCompleteAsync(10, false));

        Assert.Equal(2, ads.InterstitialShowCount);
    }

    [Fact]
    public void SessionReset_ClearsCountAndLastShown()
    {
        var ads = new CountingAdService();
        var guard = Build(ads, out _);
        guard.NotifySessionStarted();
        for (var r = 0; r < 10; r++)
        {
            guard.NotifyRunStarted();
        }

        // Manuel state check.
        Assert.Equal(10, guard.RunCount);

        guard.NotifySessionStarted();
        Assert.Equal(0, guard.SessionCount);
        Assert.Null(guard.LastShownUtc);
    }
}
