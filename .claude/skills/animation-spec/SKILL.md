---
name: animation-spec
description: Animator'ın Animation kapısında izlediği playbook. Skeletal + tween + sprite sheet entegrasyonu.
---

# Skill: animation-spec

## Ne zaman
Asset sonrası, Build sonrası. Animator yürütür.

## Ön koşul
- art-bible.md (animation style) + design.md (Juice Budget) okundu
- Asset Designer karakter sprite teslim etti
- `.claude/rules/juice.md` okundu

## Adımlar

### 1. Animation stack seçimi
- **Skeletal**: Spine Pro (karakter idle/walk/celebrate)
- **UI micro**: Rive (button press, toast appear)
- **Frame-by-frame**: sprite sheet (2-3 frame walk cycle için)
- **Tween**: MAUI Animation API veya DOTween (Unity) — code-driven

Karar: **code-driven tween birincil**, sprite sheet sadece karmaşık hareket gerekirse.

### 2. Animation kataloğu (v1.0 zorunlu)
- **Idle breath** (karakterler): y offset sin, 1.4s loop
- **Hover bounce** (CharacterSelect kartları): TranslationY 0 ↔ -4, 900ms loop, faz offset
- **Merge pop** (tile scale): 1.0 → 1.2 → 1.0, 200ms
- **Quest complete jump** (pet): TranslationY 0 → -12 → 0, 3x, 800ms
- **Biome unlock reveal**: Opacity 0 → 1 + Scale 0.8 → 1.0, 500ms
- **Fog reveal** (flood-fill): tile stagger 50ms, opacity 0 → 1
- **Page transition**: slide 300ms cubic-out
- **Popup open**: scale 0.85 → 1.0 + fade 200ms

### 3. Component: SpriteAnimator.cs
```csharp
public static class SpriteAnimator
{
    public static async Task IdleBreath(VisualElement target, CancellationToken ct);
    public static async Task HoverBounce(VisualElement target, int startDelayMs, CancellationToken ct);
    public static Task PopAnimation(VisualElement target);
    public static Task QuestCompleteJump(VisualElement target);
    public static Task UnlockReveal(VisualElement target);
    public static void StopAll(VisualElement target);
}
```

### 4. Lifecycle management
- Page OnAppearing: animation başlat (CTS)
- Page OnDisappearing: `cts.Cancel()` + `AbortAnimation` + reset transform
- Memory leak önleme: CancellationTokenSource dispose

### 5. Performance
- 60 FPS korunur
- MAUI Animation API `ScaleToAsync`/`TranslateToAsync`/`FadeToAsync` (deprecated değil)
- Scale + Translate aynı element çakışmasın (transformation sıra)
- `AbortAnimation(<key>)` sayfa çıkışında

### 6. Reduced motion
`AccessibilityPrefs.ReducedMotion == true` ise:
- Idle breath: skip
- Bounce: amplitude × 0.3
- Pop: duration × 0.5
- Unlock reveal: skip, direkt göster

### 7. Sprite sheet (opsiyonel, karmaşık hareket)
Asset Designer üretirse:
- Format: `<name>-sheet.png` yatay strip, N frame × W pixel
- Metadata: `{ "frames": 3, "width": 128, "height": 128, "fps": 8 }`
- Runtime: `int frameIndex = (int)((Stopwatch.ElapsedMilliseconds / (1000/fps)) % frames)`
- Draw: `canvas.DrawImage(sheet, src=Rect(frameIndex*W, 0, W, H), dest=...)`

### 8. BoardCanvas tile-level pop
SkiaSharp per-tile scale:
- `_popProgress` dict cellIndex → 0..1 bell curve
- `canvas.Translate(midX, midY) + Scale + Translate(-midX, -midY)`
- MAUI `Animation.Commit` bell curve: 0→0.5 progress 0→1 (cubic-out); 0.5→1 progress 1→0 (cubic-in)

### 9. Sayfa bazında entegrasyon
- **MainMenu**: SelectedPortrait idle breath
- **CharacterSelect**: 4 karakter hover bounce (faz offset 225ms/each)
- **Board**: per-tile pop on merge event
- **Board header**: pet icon quest jump
- **BiomeSelect**: newly-unlocked card reveal (Preferences "seen-unlocked" set)

## Kapanış
```
artifact_register(gameId, gate="animation", kind="code", path="games/<id>/src/<id>/Controls/SpriteAnimator.cs")
message_send(to="project-manager", type="handoff", subject="animator tamam", body="<animation list + FPS bench>")
log_append(agent="animator", gate="animation", gameId=<id>, decision="<animations + integration>", why="<tween vs sheet>")
```

## Yasaklar
- Lottie (performance.md yasak)
- Animated GIF (düşük perf)
- 60 FPS bozan animation
- Animation logic ViewModel içinde (canvas-level olmalı)
- Scale + Scale conflict (Translate + Scale karma OK)

## Done
- SpriteAnimator.cs + entegrasyon
- Board + MainMenu + BiomeSelect canlı
- FPS ≥ 55 (95%)
- Reduced motion toggle
- 1 log_append
