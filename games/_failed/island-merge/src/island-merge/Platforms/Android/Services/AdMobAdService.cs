#if ANDROID
using IslandMerge.Services;
using Microsoft.Extensions.Logging;

namespace IslandMerge.Platforms.Android.Services;

/// <summary>
/// Android AdMob placeholder. Gercek SDK entegrasyonu (Xamarin.Google.Android.PlayServices.Ads)
/// Monetization agent ile koordineli eklenir. Su an test unit ID'leri ile calisan stub uygulamasi.
/// Rewarded: ca-app-pub-3940256099942544/5224354917 (Google test rewarded)
/// Interstitial: ca-app-pub-3940256099942544/1033173712
/// </summary>
public sealed class AdMobAdService : IAdService
{
    private readonly ILogger<AdMobAdService> _logger;

    public AdMobAdService(ILogger<AdMobAdService> logger)
    {
        _logger = logger;
    }

    public bool IsInitialized { get; private set; }

    public Task InitializeAsync(CancellationToken ct = default)
    {
        // TODO (monetization agent): MobileAds.Initialize(Android.App.Application.Context, ...)
        _logger.LogInformation("AdMob placeholder initialized (no SDK bound yet)");
        IsInitialized = true;
        return Task.CompletedTask;
    }

    public async Task<AdResult> ShowRewardedAsync(AdPlacement placement, CancellationToken ct = default)
    {
        _logger.LogInformation("[AdMob stub] Show rewarded for {Placement}", placement);
        // Test env: anında reward doner. SDK bound olunca RewardedAd.Load + Show pattern.
        await Task.Delay(200, ct).ConfigureAwait(false);
        return new AdResult(Watched: true, Rewarded: true, FailReason: null);
    }

    public async Task<bool> ShowInterstitialAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("[AdMob stub] Show interstitial");
        await Task.Delay(200, ct).ConfigureAwait(false);
        return true;
    }
}
#endif
