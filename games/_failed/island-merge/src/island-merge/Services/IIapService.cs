namespace IslandMerge.Services;

public enum IapSku
{
    Energy100,
    Energy500,
    StarterPack,
    RemoveAds,
}

public readonly record struct IapResult(bool Success, string? FailReason, string? ReceiptToken);

public readonly record struct IapProduct(IapSku Sku, string Title, string Description, string PriceDisplay);

public interface IIapService
{
    bool IsInitialized { get; }

    Task InitializeAsync(CancellationToken ct = default);

    Task<IReadOnlyList<IapProduct>> GetProductsAsync(CancellationToken ct = default);

    Task<IapResult> PurchaseAsync(IapSku sku, CancellationToken ct = default);

    Task<bool> RestoreRemoveAdsAsync(CancellationToken ct = default);
}

public sealed class StubIapService : IIapService
{
    public bool IsInitialized { get; private set; }

    private static readonly IReadOnlyList<IapProduct> _products = new List<IapProduct>
    {
        new(IapSku.Energy100, "100 Enerji", "Kucuk paket", "0.99 USD"),
        new(IapSku.Energy500, "500 Enerji", "Buyuk paket (%150 deger)", "2.99 USD"),
        new(IapSku.StarterPack, "Baslangic Paketi", "500 Enerji + 200 Mucevher + kostum", "4.99 USD"),
        new(IapSku.RemoveAds, "Reklamsiz", "Tum interstitial'lari kaldirir", "3.99 USD"),
    };

    public Task InitializeAsync(CancellationToken ct = default)
    {
        IsInitialized = true;
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<IapProduct>> GetProductsAsync(CancellationToken ct = default) =>
        Task.FromResult(_products);

    public Task<IapResult> PurchaseAsync(IapSku sku, CancellationToken ct = default) =>
        Task.FromResult(new IapResult(Success: true, FailReason: null, ReceiptToken: $"stub-{sku}-{Guid.NewGuid():N}"));

    public Task<bool> RestoreRemoveAdsAsync(CancellationToken ct = default) =>
        Task.FromResult(false);
}
