namespace IslandMerge.Services;

public enum SfxKind
{
    MergePop,
    FogReveal,
    QuestComplete,
    TapUi,
    Error,
}

public interface IAudio
{
    bool Muted { get; set; }

    int Volume { get; set; } // 0..100

    void PlaySfx(SfxKind sfx);

    void PlayAmbientLoop();

    void StopAmbientLoop();
}

public sealed class NullAudio : IAudio
{
    public bool Muted { get; set; }

    public int Volume { get; set; } = 70;

    public void PlaySfx(SfxKind sfx)
    {
        // No-op placeholder. Gerçek SFX asset'leri pipeline'da geldiğinde doldurulur.
    }

    public void PlayAmbientLoop()
    {
    }

    public void StopAmbientLoop()
    {
    }
}
