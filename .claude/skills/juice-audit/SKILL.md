---
name: juice-audit
description: Game Feel Engineer'ın Juice kapısında izlediği playbook. Design.md Juice Budget matrisini koda döker. 5 zorunlu teknik + QA test.
---

# Skill: juice-audit

## Ne zaman
Build tamamlandıktan sonra. PM Game Feel Engineer'ı çağırır.

## Ön koşul
- `design.md` Juice Budget matrisi doldurulmuş
- Build yeşil
- `.claude/rules/juice.md` okundu

## Adımlar

### 1. Matrisin yorumu
design.md'deki tabloyu oku. Her event için 5 kanal hedefi:
- Visual, Sound, Haptic, Screen Shake, Hit Stop

### 2. Component implementation

#### Controls/ParticleSystem.cs
Pool-based, max 80 particle, GPU-friendly SkiaSharp draw.
```csharp
public sealed class ParticleSystem : SKCanvasView
{
    public void Emit(SKPoint at, int count = 12, SKColor color = default, float lifetime = 0.8f);
    public void EmitBurst(SKPoint at, int count, float spread, SKColor[] palette);
}
```

#### Controls/ScreenShake.cs
Page-level Perlin translation.
```csharp
public sealed class ScreenShake
{
    public void Shake(float amplitude, float frequency, int durationMs);
    // Perlin noise sample, UI thread
}
```

#### Controls/HitStop.cs
Task.Delay wrapper, UI-safe.
```csharp
public static class HitStop
{
    public static Task Pause(int ms) => Task.Delay(ms); // simple
    // Advanced: time scale freeze gameLogic loop
}
```

#### Controls/HapticService.cs
Platform wrapper.
```csharp
public sealed class HapticService
{
    public void Light() => HapticFeedback.Default.Perform(HapticFeedbackType.Click);
    public void Medium() => HapticFeedback.Default.Perform(HapticFeedbackType.Click);
    public void Strong() => HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
}
```

#### Controls/ButtonJuice.cs (attached behavior)
Her primary button için:
- Scale 1.0 → 0.95 → 1.08 → 1.0 (ease-out-back, 120ms)
- Haptic light on press
- SFX btn_tap.ogg on release

### 3. Pitch variance (SFX)
`AudioService.Play(name, pitchVariance = 0.1)` → `PlaybackRate = Random.Shared.NextDouble() * 0.2 + 0.9`

Combo chain semitone ladder:
```csharp
float pitch = MathF.Pow(2, semitones / 12f); // chain 1 = 0, chain 2 = +1, ...
```

### 4. Entegrasyon
Juice Budget matrisindeki her satır için kod bağla:

```csharp
// BoardViewModel.OnMergeSuccess
_particles.EmitBurst(mergeCell, 20, spread: 45, palette: tierColors);
await _screenShake.ShakeAsync(3, 20, 200);
await HitStop.Pause(50);
_haptic.Medium();
_audio.Play("merge_pop", pitchVariance: 0.1);
// 5 kanal ✓
```

### 5. Performance check
60 FPS korunuyor mu? Test:
- 5 merge ardışık (combo)
- Particle + shake + hit stop kombinasyonu
- SKCanvasView invalidate sadece dirty frame'de

Eğer <55 FPS → particle budget azalt (80→40), hit stop kısalt.

### 6. Reduce-motion accessibility
`AccessibilityPrefs.ReducedMotion == true` ise:
- Particle count × 0.3
- Screen shake amplitude × 0.3
- Hit stop duration × 0.5

Settings toggle ile kontrol.

### 7. Juice Budget coverage tablosu
Her event için check:

| Event | Visual ✓ | Sound ✓ | Haptic ✓ | Shake ✓ | HitStop ✓ |
|---|---|---|---|---|---|
| Tap | ✓ | ✓ | ✓ | — | — |
| Merge | ✓ | ✓ | ✓ | ✓ | ✓ |
| ... |

Eksik hücre = P1 bug.

### 8. Unit test (Juice coverage)
`JuiceBudgetTests.cs`:
- Assert: her event ≥3 kanal
- Assert: reduced motion toggle çalışıyor
- Assert: FPS bench stub (statik analiz)

## Kapanış
```
artifact_register(gameId, gate="juice", kind="code", path="games/<id>/src/<id>/Controls/")
message_send(to="project-manager", type="handoff", subject="juice tamam", body="<coverage % + FPS bench>")
log_append(agent="game-feel-engineer", gate="juice", gameId=<id>, decision="<coverage + perf>", why="<Budget uyumu>")
```

## Yasaklar
- Juice overload (her frame particle)
- Seizure flash (>3 Hz full-screen)
- Setup sırasında hardcode sound path (audio service abstraction)
- Haptic zorla on
- Lottie ekleme (performance.md yasağı)

## Done
- Juice Budget matrisi %100 implement
- 60 FPS korunur
- Reduce-motion toggle
- Haptic opt-out
- 1 log_append
