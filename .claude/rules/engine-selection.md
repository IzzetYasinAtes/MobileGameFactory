# Oyun Motoru Seçimi

**Kural #1**: Motor oyunun türüne göre seçilir, kütüphaneye duygusal bağlılık olmaz.

## Karar matrisi

| Genre | Önerilen motor | Alternatif | Yasak |
|---|---|---|---|
| Match-3 / Merge | **Unity** / Godot 4 | .NET MAUI + SkiaSharp | — |
| Hyper-casual | Unity / **Godot 4** | Defold | — |
| Idle / Clicker | MAUI + SkiaSharp | Unity, Godot | — |
| Puzzle (premium) | Godot / Unity | MAUI | — |
| Word / Card / Board | **MAUI + SkiaSharp** | Flutter Casual Games Toolkit | Unity (overkill) |
| Turn-based RPG | Unity | Godot 4 | MAUI |
| Reflex / Platformer | **Unity** | Godot 4 | MAUI (perf + tooling) |
| 3D casual | Unity | Unreal Mobile | MAUI |
| AR | **Unity ARFoundation** | — | MAUI, Godot (immature AR) |
| Rhythm / music | Unity | Godot | MAUI (audio latency) |

## .NET MAUI + SkiaSharp — ne zaman kullanılır

**UYGUN**:
- Puzzle (sudoku, crossword, solitaire)
- Idle / clicker (tap-centric)
- Word / card / board (turn-based minimal animation)
- Quiz / trivia
- Story-rich narrative-heavy

**UYGUN DEĞIL**:
- Match-3 / merge particle-heavy (engine friction → ship'i keser)
- Reflex / arcade (60 FPS particle + shake + FX zor)
- 3D her tür
- AR / VR
- Physics-heavy (ragdoll, collision, rigid body)
- Custom shader / compute heavy

## Unity — casual/mid-core mobil için

- **Pro**: ekosystem (Asset Store, Spine, DOTween, Cinemachine), particle system, shader graph, Play Billing + AdMob SDK olgun, analytics SDK (Firebase, Appsflyer), Play Asset Delivery, Unity Cloud Build
- **Con**: proje boyutu büyük (APK ~20 MB baseline), lisans (Plus $399/yr, $200K revenue altında ücretsiz), generic C# MVVM zor (custom framework)
- **Kullanım senaryosu**: team ≥2 kişi, game feel critical, Spine/particle/shader şart

## Godot 4 — indie casual

- **Pro**: MIT lisansı (tam ücretsiz), 40 MB editor, GDScript + C# support, Godot 4.x C# olgunlaştı, mobile export olgun, renderer 2D + 3D
- **Con**: ekosystem Unity'den küçük (Asset Store analog Godot Asset Library sınırlı), Spine runtime resmi değil (community), analytics SDK sınırlı
- **Kullanım senaryosu**: solo dev, casual puzzle, open source zihniyeti

## MAUI tercih edilirse dikkat
- SkiaSharp `SKCanvasView` GPU-accelerated (SKGLView)
- AOT+trim ship build (cold start <2s, APK <40 MB)
- Ad SDK: AdMob **wrapper** var ama olgunluk sınırlı — test edilir
- Play Billing: Plugin.Maui.InAppBilling kullanılabilir
- Spine runtime: resmi MAUI yok → Rive kullan veya frame-by-frame
- Particle system: custom SkiaSharp implementation
- Ship-blocker test: cihaz üzerinde 5 dk sürekli oyun + 60 FPS korundu mu

## Soft launch ile motor doğrulama
Launch'tan önce soft launch (TR veya PH/ID):
- Crash-free rate ≥99.5%
- D1 R ≥35%, D7 R ≥12%
- Motor-bağlı issue varsa (ör. particle lag, audio clip) → motor değişikliği bile değerlendirilir

## Kill kriter
Eğer motor oyun ölçeğinin %80'ini getiriyor, %20 ship'i engelliyor → kill ya da motor rework. Bu durum `polish-or-kill` gate'inde tespit edilir.
