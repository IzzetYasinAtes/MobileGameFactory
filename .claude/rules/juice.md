# Game Feel / Juice Kuralları

**"Juice subjektif değildir — ölçülebilir bir matris."**
Her oyun design.md içinde `Juice Budget` matrisi doldurur. Eksik hücre = P1 bug, QA kapısı blokajı.

## Juice Budget matrisi (zorunlu)

Her event için 5 kanalda feedback tanımlı olmalı. Event listesi örnektir, oyuna göre uzatılır.

| Event | Visual | Sound | Haptic | Screen Shake | Hit Stop |
|---|---|---|---|---|---|
| Tap (button) | scale 1→1.1→1 ease-out-back 120ms | btn_click.wav | light | — | — |
| Match / Merge | particle burst 12–20 + color flash 100ms | merge_pop + harmonic +1 semitone | medium | 3px 20Hz 200ms | 50ms |
| Combo chain (n≥3) | multi-particle rainbow trail | merge_pop + pitch +5% per step | medium+ | 5px 25Hz 250ms | 80ms |
| Quest complete | confetti 40+ + slow-mo 0.3s | fanfare_short.wav | strong | 6px 250ms | 100ms |
| Level/world unlock | flood-fill reveal + fireworks | unlock_chime.wav | strong | 8px 300ms | 120ms |
| Coin pickup | spark + float to HUD | coin_ding randomized pitch ±10% | soft | — | — |
| Damage / fail | red screen tint 150ms + shake | fail_buzz.wav | strong | 8px 30Hz 350ms | 100ms |
| Rewarded ad complete | +50 energy fly-in particle trail | reward_tada.wav | medium | — | — |

## 5 olmazsa olmaz juice teknik (sektör standardı)

### 1. Squash & stretch (ease-out-back)
- Her tap/press/button'da 80–120ms scale tween
- Ease: `CubicOut` → `Back` (overshoot %10)
- En ucuz juice, en büyük "snappy" etkisi
- Referans: Royal Match primary button

### 2. Multi-channel event feedback
- Kritik olay (merge, match, score) min **3 kanaldan** bildirilir:
  - Particle burst (8–20 parça, gradient fade, 0.5–1.2s lifetime)
  - Color flash (tile rengi → beyaz → tile rengi, 100ms)
  - Haptic impact (iOS: `UIImpactFeedbackGenerator .medium`; Android: `VibrationEffect.EFFECT_CLICK`)
- Jonasson/Purho "Juice it or lose it" ana dersi

### 3. Hit stop / freeze frame
- Merge veya büyük skorda zaman dur
- **30–100ms, sweet spot 50ms** (insan algı eşiği)
- "Weight" hissinin tek kaynağı
- Implementasyon: `Task.Delay(50)` UI coroutine'de veya `Animation.Commit` beforePause

### 4. Screen shake (Perlin noise)
- **2–8 px amplitude**, event ağırlığına göre
- **20–30 Hz frequency** (Perlin, sine DEĞIL — mide bulantısı düşük)
- **Max 250–350ms cap** — daha uzun = oyuncu rahatsız
- Implementasyon: `canvas.Translate(perlin(t)*amp, perlin(t+100)*amp)` her frame

### 5. SFX pitch variance + layering
- Tekrar eden seste **±10% pitch randomize** (monotonluk kırılır)
- Combo chain'de **yarım-ton yukarı merdiven** (+1, +2, +3 semitone)
- Layer: **attack (transient)** + **body (tone)** — ikisi birlikte çalar
- Asset: OGG compressed, ≤48 kbps SFX için yeterli

## Ekstra juice (stretch goal)

- Ghost trail (moving item'da)
- Breathing idle (karakter portrait idle'da y-offset sin)
- Particle ambient (world bg'da floating leaves/sparkles)
- Camera zoom on big event (2% zoom, 200ms, return 300ms)
- Confetti pool (performance için 80 max particle, recycled)

## Anti-pattern'ler (YASAK)

- Juice overload — her frame particle → 60 FPS bozulur
- Seizure flash (full-screen beyaz flash ≥3 kez/saniye) — PEGI warning + sağlık riski
- Laggy animation (setTimeout 500ms değil, gerçek interpolation)
- Sound spam (aynı SFX 10ms içinde tekrar → clipping — cooldown zorunlu)
- Screen shake zoom-heavy — mobil ekranda başağrısı

## Implementasyon stacki (MAUI)

- **Tween**: `MAUI Animation API` (`this.TranslateToAsync`, `ScaleToAsync`), veya SkiaSharp manual `Animation.Commit`
- **Particle**: SkiaSharp custom particle system (pool-based, ≤80 max)
- **Haptic**: `HapticFeedback.Default.Perform(HapticFeedbackType.Click|LongPress)`
- **SFX**: `Plugin.Maui.Audio` + randomized pitch (playback rate)
- **Screen shake**: page-level `TranslationX/Y` tween with Perlin-based sample

## QA check (ship öncesi)
- [ ] Juice Budget matrisi doluysa
- [ ] Her event için min 3 kanal feedback var mı
- [ ] 60 FPS korunuyor mu (particle burst + shake + hit stop kombinasyonunda)
- [ ] Reduce-motion accessibility toggle respected mı (shake + particle azaltılır)
- [ ] Seizure test: 3 Hz flash limit aşılmadı mı
- [ ] Haptic opt-out setting var mı

## Referanslar
- Steve Swink — *Game Feel: A Game Designer's Guide to Virtual Sensation*
- Martin Jonasson & Petri Purho — "Juice it or lose it" (GDC 2012)
- Royal Match / Homescapes / Candy Crush teardown (deconstructoroffun)
