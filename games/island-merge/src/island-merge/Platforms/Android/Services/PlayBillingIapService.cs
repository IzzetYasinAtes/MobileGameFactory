#if ANDROID
using IslandMerge.Services;
using Microsoft.Extensions.Logging;

namespace IslandMerge.Platforms.Android.Services;

/// <summary>
/// Android Google Play Billing placeholder. Gercek BillingClient entegrasyonu
/// Monetization agent ile koordineli eklenir.
/// </summary>
public sealed class PlayBillingIapService : IIapService
{
    private readonly ILogger<PlayBillingIapService> _logger;

    private static readonly IReadOnlyList<IapProduct> _products = new List<IapProduct>
    {
        new(IapSku.Energy100, "100 Enerji", "Kucuk paket", "0.99 USD"),
        new(IapSku.Energy500, "500 Enerji", "Buyuk paket (%150 deger)", "2.99 USD"),
        new(IapSku.StarterPack, "Baslangic Paketi", "500 Enerji + 200 Mucevher + kostum", "4.99 USD"),
        new(IapSku.RemoveAds, "Reklamsiz", "Tum interstitial'lari kaldirir", "3.99 USD"),
    };

    public PlayBillingIapService(ILogger<PlayBillingIapService> logger)
    {
        _logger = logger;
    }

    public bool IsInitialized { get; private set; }

    public Task InitializeAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Play Billing placeholder initialized (no SDK bound yet)");
        IsInitialized = true;
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<IapProduct>> GetProductsAsync(CancellationToken ct = default) =>
        Task.FromResult(_products);

    public async Task<IapResult> PurchaseAsync(IapSku sku, CancellationToken ct = default)
    {
        _logger.LogInformation("[Play Billing stub] Purchase {Sku}", sku);
        await Task.Delay(300, ct).ConfigureAwait(false);
        return new IapResult(Success: true, FailReason: null, ReceiptToken: $"play-stub-{sku}-{Guid.NewGuid():N}");
    }

    public Task<bool> RestoreRemoveAdsAsync(CancellationToken ct = default) =>
        Task.FromResult(false);
}
#endif
