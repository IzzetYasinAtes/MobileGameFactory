using Microsoft.Maui.Controls;

namespace IslandMerge.Controls;

/// <summary>
/// Reusable 2D code-driven tween animations for Island Merge.
/// MAUI Animation API (ScaleToAsync/TranslateToAsync/FadeToAsync) - UI thread, 60 FPS safe.
/// Sprite sheet DEGIL; runtime tween.
///
/// Loop animation'larda CancellationToken verilmezse sonsuz doner,
/// cagiran taraf VisualElement.AbortAnimation(key) ile animasyon anahtarini
/// bastirmali. Kolaylik icin StopAll temizleme saglar.
/// </summary>
public static class SpriteAnimator
{
    // Animation key'leri - AbortAnimation ile temizlemek icin sabit.
    public const string KeyIdleBreath = "im_idle_breath";
    public const string KeyHoverBounce = "im_hover_bounce";
    public const string KeyPop = "im_pop";
    public const string KeyJump = "im_quest_jump";
    public const string KeyReveal = "im_unlock_reveal";

    /// <summary>
    /// Idle breath: scale 1.0 to 1.03 and back, 1.4s loop, Easing.SinInOut, sonsuz.
    /// CancellationToken iptal edilince Scale = 1.0 restore.
    /// </summary>
    public static async Task IdleBreathAnimation(VisualElement target, CancellationToken ct = default)
    {
        if (target is null)
        {
            return;
        }

        const uint halfPeriodMs = 700; // 1.4s toplam, her yon 700ms
        target.AnchorX = 0.5;
        target.AnchorY = 0.5;

        try
        {
            while (!ct.IsCancellationRequested)
            {
                await target.ScaleToAsync(1.03, halfPeriodMs, Easing.SinInOut).ConfigureAwait(true);
                if (ct.IsCancellationRequested)
                {
                    break;
                }
                await target.ScaleToAsync(1.0, halfPeriodMs, Easing.SinInOut).ConfigureAwait(true);
            }
        }
        catch (TaskCanceledException)
        {
            // sessizce cik - iptal normal.
        }
        finally
        {
            target.AbortAnimation(KeyIdleBreath);
            target.Scale = 1.0;
        }
    }

    /// <summary>
    /// Hover bounce: TranslationY 0 to -4 and back, 900ms loop, Easing.SinInOut.
    /// startDelayMs ile faz offset verilebilir (coklu karakter eszamanli olmasin).
    /// </summary>
    public static async Task HoverBounceAnimation(
        VisualElement target,
        int startDelayMs = 0,
        CancellationToken ct = default)
    {
        if (target is null)
        {
            return;
        }

        const uint halfPeriodMs = 450; // 900ms toplam

        try
        {
            if (startDelayMs > 0)
            {
                await Task.Delay(startDelayMs, ct).ConfigureAwait(true);
            }

            while (!ct.IsCancellationRequested)
            {
                await target.TranslateToAsync(target.TranslationX, -4, halfPeriodMs, Easing.SinInOut)
                    .ConfigureAwait(true);
                if (ct.IsCancellationRequested)
                {
                    break;
                }
                await target.TranslateToAsync(target.TranslationX, 0, halfPeriodMs, Easing.SinInOut)
                    .ConfigureAwait(true);
            }
        }
        catch (TaskCanceledException)
        {
            // iptal normal
        }
        finally
        {
            target.AbortAnimation(KeyHoverBounce);
            target.TranslationY = 0;
        }
    }

    /// <summary>
    /// Pop: scale 1.0 to 1.2 to 1.0, 200ms (merge feedback).
    /// Tek atis, tamamlanir.
    /// </summary>
    public static async Task PopAnimation(VisualElement target)
    {
        if (target is null)
        {
            return;
        }

        target.AnchorX = 0.5;
        target.AnchorY = 0.5;

        await target.ScaleToAsync(1.2, 100, Easing.CubicOut).ConfigureAwait(true);
        await target.ScaleToAsync(1.0, 100, Easing.CubicIn).ConfigureAwait(true);
    }

    /// <summary>
    /// Quest complete: TranslationY 0 to -12 to 0, 3 kez, 800ms toplam.
    /// Her zipla ~267ms. Easing.CubicOut yukari, CubicIn asagi.
    /// </summary>
    public static async Task QuestCompleteJump(VisualElement target)
    {
        if (target is null)
        {
            return;
        }

        const uint upMs = 130;
        const uint downMs = 135;

        for (var i = 0; i < 3; i++)
        {
            await target.TranslateToAsync(target.TranslationX, -12, upMs, Easing.CubicOut).ConfigureAwait(true);
            await target.TranslateToAsync(target.TranslationX, 0, downMs, Easing.CubicIn).ConfigureAwait(true);
        }
    }

    /// <summary>
    /// Unlock reveal: Opacity 0 to 1 + Scale 0.8 to 1.0, 500ms, Easing.CubicOut.
    /// Baslamadan once target Opacity ve Scale degerleri 0/0.8'e sifirlanir.
    /// </summary>
    public static async Task UnlockReveal(VisualElement target)
    {
        if (target is null)
        {
            return;
        }

        target.AnchorX = 0.5;
        target.AnchorY = 0.5;
        target.Opacity = 0;
        target.Scale = 0.8;

        var fade = target.FadeToAsync(1.0, 500, Easing.CubicOut);
        var scale = target.ScaleToAsync(1.0, 500, Easing.CubicOut);
        await Task.WhenAll(fade, scale).ConfigureAwait(true);
    }

    /// <summary>
    /// Page leave helper: bilinen tum animation key'lerini bastirir + transform reset.
    /// </summary>
    public static void StopAll(VisualElement target)
    {
        if (target is null)
        {
            return;
        }

        target.AbortAnimation(KeyIdleBreath);
        target.AbortAnimation(KeyHoverBounce);
        target.AbortAnimation(KeyPop);
        target.AbortAnimation(KeyJump);
        target.AbortAnimation(KeyReveal);
        target.Scale = 1.0;
        target.TranslationY = 0;
        target.Opacity = 1.0;
    }
}
