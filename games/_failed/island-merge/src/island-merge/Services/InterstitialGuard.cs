using Microsoft.Extensions.Logging;

namespace IslandMerge.Services;

/// <summary>
/// Interstitial frekans disiplini. monetization.md kurallari:
///  - RemoveAds satin alindiysa asla gosterilmez.
///  - Ilk 3 run icinde gosterilmez.
///  - L4 altinda gosterilmez.
///  - Session hard cap: 2.
///  - Placement cooldown: 4 dakika.
/// </summary>
public interface IInterstitialGuard
{
    /// <summary>Run basladiginda (oyun acilisi / seviyeye giris) cagir.</summary>
    void NotifyRunStarted();

    /// <summary>Yeni uygulama session'i basladiginda sifirla.</summary>
    void NotifySessionStarted();

    /// <summary>Level complete sonrasi interstitial denemeyi sarabilen tek nokta.</summary>
    Task<bool> TryShowOnLevelCompleteAsync(int currentLevel, bool removeAdsPurchased, CancellationToken ct = default);
}

public sealed class InterstitialGuard : IInterstitialGuard
{
    public const int MinLevel = 4;
    public const int FirstRunsWithoutAd = 3;
    public const int SessionHardCap = 2;
    public static readonly TimeSpan MinInterval = TimeSpan.FromMinutes(4);

    private readonly IAdService _ads;
    private readonly ILogger<InterstitialGuard> _logger;
    private readonly Func<DateTimeOffset> _now;

    private int _runCount;
    private int _sessionCount;
    private DateTimeOffset? _lastShownUtc;

    public InterstitialGuard(IAdService ads, ILogger<InterstitialGuard> logger)
        : this(ads, logger, () => DateTimeOffset.UtcNow)
    {
    }

    // Test hook.
    internal InterstitialGuard(IAdService ads, ILogger<InterstitialGuard> logger, Func<DateTimeOffset> nowProvider)
    {
        _ads = ads;
        _logger = logger;
        _now = nowProvider;
    }

    internal int RunCount => _runCount;

    internal int SessionCount => _sessionCount;

    internal DateTimeOffset? LastShownUtc => _lastShownUtc;

    public void NotifyRunStarted() => _runCount++;

    public void NotifySessionStarted()
    {
        _sessionCount = 0;
        _lastShownUtc = null;
    }

    public async Task<bool> TryShowOnLevelCompleteAsync(int currentLevel, bool removeAdsPurchased, CancellationToken ct = default)
    {
        if (removeAdsPurchased)
        {
            _logger.LogDebug("Interstitial skipped: remove_ads purchased");
            return false;
        }

        if (currentLevel < MinLevel)
        {
            _logger.LogDebug("Interstitial skipped: level {Level} below min {Min}", currentLevel, MinLevel);
            return false;
        }

        if (_runCount <= FirstRunsWithoutAd)
        {
            _logger.LogDebug("Interstitial skipped: early run #{Run}", _runCount);
            return false;
        }

        if (_sessionCount >= SessionHardCap)
        {
            _logger.LogDebug("Interstitial skipped: session cap reached ({Cap})", SessionHardCap);
            return false;
        }

        var now = _now();
        if (_lastShownUtc is { } last && (now - last) < MinInterval)
        {
            _logger.LogDebug("Interstitial skipped: interval not met (since {Elapsed})", now - last);
            return false;
        }

        try
        {
            var shown = await _ads.ShowInterstitialAsync(ct).ConfigureAwait(false);
            if (shown)
            {
                _sessionCount++;
                _lastShownUtc = now;
                _logger.LogInformation("Interstitial shown (session {Count}/{Cap})", _sessionCount, SessionHardCap);
            }
            return shown;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Interstitial show failed");
            return false;
        }
    }
}
