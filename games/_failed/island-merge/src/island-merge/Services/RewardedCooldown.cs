namespace IslandMerge.Services;

/// <summary>
/// Rewarded ad placement cooldown (monetization.md): aynı placement için 30s.
/// Farklı placement'lar bağımsız sayilir.
/// </summary>
public interface IRewardedCooldown
{
    TimeSpan PlacementCooldown { get; }

    bool IsReady(AdPlacement placement);

    void NotifyShown(AdPlacement placement);

    TimeSpan? TimeLeft(AdPlacement placement);
}

public sealed class RewardedCooldown : IRewardedCooldown
{
    public TimeSpan PlacementCooldown { get; } = TimeSpan.FromSeconds(30);

    private readonly Dictionary<AdPlacement, DateTimeOffset> _lastShown = new();
    private readonly Func<DateTimeOffset> _now;

    public RewardedCooldown()
        : this(() => DateTimeOffset.UtcNow)
    {
    }

    internal RewardedCooldown(Func<DateTimeOffset> nowProvider)
    {
        _now = nowProvider;
    }

    public bool IsReady(AdPlacement placement) => TimeLeft(placement) is null;

    public void NotifyShown(AdPlacement placement) => _lastShown[placement] = _now();

    public TimeSpan? TimeLeft(AdPlacement placement)
    {
        if (!_lastShown.TryGetValue(placement, out var last))
        {
            return null;
        }

        var elapsed = _now() - last;
        if (elapsed >= PlacementCooldown)
        {
            return null;
        }
        return PlacementCooldown - elapsed;
    }
}
