# Game Feel & Juice — Research Raporu

**Tarih:** 2026-04-23
**Kapsam:** Mobil oyun üretimi için "game feel" ve "juice" teknikleri, implementasyon referansları ve anti-patternler.
**Hedef kitle:** MAUI Developer, Game Designer, QA Tester.

---

## 1. Temel teori — Steve Swink, *Game Feel* (2008)

Swink'in üç sütunu oyun hissinin tanımıdır:

1. **Real-time control** — girdi ile tepki arasında ölçülebilir gecikme (< 100 ms ideal, < 150 ms maksimum tolere edilebilir).
2. **Simulated space** — sanal nesnelerin fizik-benzeri etkileşimi; çarpışma, momentum, atalet.
3. **Polish** — simülasyonu değiştirmeden (screen shake, particle, SFX) duyusal katmanı güçlendiren her şey.

Swink'in metriği: *input latency*, *response curve*, *directness*. Her kapı bu üçlüye bakmalı.

---

## 2. Pratik — "Juice it or lose it" (Jonasson & Purho, GDC Europe 2012)

Breakout klonunu sahnede juice ile dönüştürerek şu teknikleri kanıtladılar:
- Paddle çarpınca **tween scale** (squash 0.7x, stretch 1.3x, 120 ms ease-out).
- Brick kırılınca **particle burst** + **color flash** + **screen shake** + **SFX pitch variance** + **ball trail**.
- Ekran her olayda "yaşıyor" — durağan frame yok.

Ders: **aynı olayı 5–7 ayrı kanaldan bildir** (görsel + ses + haptic + animasyon + kamera). Her kanal başına maliyet düşük, toplam etki katlanır.

---

## 3. Juice checklist (25 madde — her MAUI oyunda gözden geçir)

### Görsel polish
1. **Screen shake** — amplitude 2–8 px, frequency 20–30 Hz, duration 100–250 ms. Perlin noise tercih (sine dönüşlü, nausea düşük). Intensity event ağırlığıyla orantılı.
2. **Particle burst** — 8–20 particle, 250–600 ms lifetime, gradient color (alpha 1.0 → 0.0, size 1.0 → 0.3), radial velocity randomize. Mobilde count ≤ 25, texture 64×64 atlas.
3. **Color flash** — 80–150 ms, alpha 0.3–0.6 beyaz veya event-renk overlay. Ease-out.
4. **Freeze frame / hit stop** — 30–100 ms (merge, crit, big hit). İnsan algı eşiği ~100 ms; 50 ms sweet spot. Physics pause + animation pause + sound continues.
5. **Squash & stretch** — button press 0.9x 80 ms → 1.0x 120 ms ease-out back. Bouncing orb 1.2x/0.8x ratio, volume korunur.
6. **Anticipation** — büyük aksiyondan önce 80–120 ms ters hareket (shot öncesi geri çekilme, jump öncesi çömelme).
7. **Ghosting / motion trail** — hızlı hareket eden nesnede 4–6 kopya, alpha sönümlü (1.0, 0.6, 0.3, 0.15), 50 ms aralıkla.
8. **Camera zoom punch** — kritik anda 1.0 → 1.03 → 1.0, toplam 200 ms, ease-in-out. Gameplay'i bozmaz.
9. **Ease curves** — lineer kullanma. Varsayılan: `ease-out-cubic` (giriş), `ease-in-out-quad` (continuous), `ease-out-back` (snap/pop). DOTween/Tween karşılıkları hazır.
10. **Damage / score number pop** — spawn scale 0 → 1.2 → 1.0 (ease-out-back, 200 ms), rise 40 px, fade out 400 ms. Renk-kodlu (crit altın, normal beyaz).

### Ses polish
11. **SFX layering** — her event min 2 katman: *attack* (kısa transient, ≤ 50 ms) + *body* (100–300 ms). Merge: pluck + sparkle; explosion: punch + boom + shimmer.
12. **Pitch variance** — aynı SFX üst üste tekrarlanırsa ±10% random pitch; "machine gun" etkisi dağılır.
13. **Volume ducking** — büyük event 100 ms music duck (-6 dB), sonra 300 ms attack.

### Haptic (mobil-spesifik)
14. **iOS UIImpactFeedbackGenerator** — light (button press), medium (merge/match), heavy (level complete, rare). Selection feedback swipe için.
15. **Android Vibrator** — `VibrationEffect.createOneShot(30ms, DEFAULT_AMPLITUDE)` tap; compositional primitive API 30+.
16. **Haptic bütçesi** — session başına ~30 haptic event tavan; hepsi aynı ağırlıkta olmasın. "Less is more" — Apple ve Android rehberleri ortak.

### Mobil-spesifik polish
17. **Tap ripple** — temas noktasından 0 → 80 px radius, 300 ms, alpha 0.4 → 0.0. Material Design referansı.
18. **Button press scale** — down 0.95x 60 ms, up 1.0x 120 ms ease-out-back.
19. **Swipe trail** — parmak yolu boyunca 6–10 particle fade, gradient renk eventle eşleşir.
20. **Match-3 chain VFX** — her zincir için escalating feedback: 3-match sade pop, 4-match line beam, 5-match radial blast, combo chain başına pitch yarım-ton yukarı.
21. **Coin / pickup burst** — ekrana 6–10 coin saçılır (bezier path ile HUD coin counter'ına akar), 400–600 ms staggered, counter tick SFX.
22. **Level up celebration** — 3 katmanlı: confetti (400+ particle 1.5 s), sunburst background flash (300 ms), banner slide-in (ease-out-back, 500 ms). Frekans düşük tutulmalı ki "big moment" kalsın.
23. **Damage number pop** — bkz. madde 10.

### Anti-patternler (NO-GO listesi)
24. **Juice overload** — her event full-stack juice alırsa hiyerarşi çöker. Yalnız "çekirdek loop kapanışı" için full shake+flash+haptic; ara event'ler tek kanal.
25. **Erişilebilirlik ihlalleri** — saniyede > 3 flash (WCAG) yasak; "reduce motion" toggle zorunlu (iOS `UIAccessibility.isReduceMotionEnabled`, Android `ANIMATOR_DURATION_SCALE`). Seizure riski: hızlı kırmızı/beyaz flash yasak. Screen shake kapatılabilir olmalı.

---

## 4. Popüler oyun analizleri

- **Royal Match (Dream Games)** — tüm match-3 pazarının juice referansı. Kısa/akıcı swap animasyonu, her obstacle için ayrı VFX kimliği, combo'da cascade ses merdiveni, UI'da küçük ama sürekli micro-animations. "Snappy" tanımının prototipi.
- **Homescapes / Gardenscapes (Playrix)** — karakter reaction animations, level complete'te ayrı sahne bile oynatır; meta progression juice gameplay juice kadar önemli.
- **Candy Crush Saga (King)** — cascade sistemi; her combo chain sesi yarım-ton yukarı, voice-over ("Sweet!", "Delicious!") rewarding.
- **Monument Valley (ustwo)** — juice *azlığı* üzerinden juice. Her geometri dönüşünde tek temiz soft chime + gentle camera drift. Minimalist juice de juice.
- **Alto's Adventure (Snowman)** — Taptic Engine entegrasyonu llama toplama, ice boost, kamera min/max zoom için; paper lantern, bird scatter ambient juice; combo'da golden burst.

---

## 5. Implementasyon referansları

- **Unity** — Cinemachine (impulse source screen shake), DOTween (`.DOScale`, `.DOPunchPosition`, `.DOShakeRotation`), Feel/MMTools asset (Corgi) juice kütüphanesi.
- **Godot** — `Tween` node, shader `fragment()` flash, `AudioStreamPlayer.pitch_scale` random.
- **Bizim stack: MAUI + SkiaSharp** — özel tween: `PeriodicTimer(16ms)` → `InvalidateSurface()`. `SKCanvas` ile transform (scale/translate/rotate), `SKShader` ile flash overlay, `SKPaint.BlendMode.Plus` glow. Haptic: `HapticFeedback.Default.Perform(HapticFeedbackType.Click)` MAUI Essentials.

SkiaSharp tween helper deseni (pseudo):
```
record Tween(float From, float To, float Duration, EaseFn Ease);
float Eval(Tween t, float elapsed) => t.From + (t.To - t.From) * t.Ease(elapsed / t.Duration);
// OnPaintSurface'te her frame güncel değeri çiz, bittiğinde listeyi temizle.
```

Performans: particle count ≤ 25 aynı anda, texture atlas cache, allocation sıcak yolda `0`, `Span<T>` ile pool. `.claude/rules/performance.md` 60 FPS şartı.

---

## 6. Sistem kuralı önerisi (MobileGameFactory'ye)

Her `games/<id>/design.md` "Juice Budget" tablosu içermeli: event × kanal matrisi (visual/sound/haptic/shake/freeze). QA kapısında bu tablo gerçek build ile karşılaştırılır; eksik satır = P1 bug. Böylece juice subjektif değil *audit edilebilir* olur.

---

**Kaynaklar:** GDC Vault "Juice It or Lose It" talk, Steve Swink *Game Feel* (Morgan Kaufmann), Snowman devblog (Alto Haptics), Dream Games Royal Match teardowns (Naavik, Deconstructor of Fun), Wayline "Juice Overload" serisi, Apple HIG Haptics, Android Haptics Design Principles, SkiaSharp docs, Game Accessibility Guidelines (flicker).
