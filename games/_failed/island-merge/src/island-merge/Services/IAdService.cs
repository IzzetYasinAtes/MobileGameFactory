namespace IslandMerge.Services;

public enum AdPlacement
{
    EnergyRefill,
    LootDouble,
    ProducerSkip,
    InterstitialReturn,
}

public readonly record struct AdResult(bool Watched, bool Rewarded, string? FailReason);

public interface IAdService
{
    bool IsInitialized { get; }

    Task InitializeAsync(CancellationToken ct = default);

    Task<AdResult> ShowRewardedAsync(AdPlacement placement, CancellationToken ct = default);

    Task<bool> ShowInterstitialAsync(CancellationToken ct = default);
}

/// <summary>
/// Dev / test stub — her zaman basarili rewarded doner.
/// Windows/iOS/Mac build'lerinde default.
/// </summary>
public sealed class StubAdService : IAdService
{
    public bool IsInitialized { get; private set; }

    public Task InitializeAsync(CancellationToken ct = default)
    {
        IsInitialized = true;
        return Task.CompletedTask;
    }

    public Task<AdResult> ShowRewardedAsync(AdPlacement placement, CancellationToken ct = default) =>
        Task.FromResult(new AdResult(Watched: true, Rewarded: true, FailReason: null));

    public Task<bool> ShowInterstitialAsync(CancellationToken ct = default) =>
        Task.FromResult(true);
}
