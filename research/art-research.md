# Art Research — Mobile Game Art Pipeline (2026)

Kaynak tarih: 2026-04-24. Hedef: MobileGameFactory'de AAA kalite 2D casual oyun teslim edilebilecek art pipeline iskeletini kurmak.

---

## 1. Art Bible — zorunlu ana belge

Art bible oyunun görsel kimliğini kilitleyen referans dokümanıdır. 2026'da cross-platform launch talepleri arttıkça (mağaza capsule, reveal trailer, regional beat, live content) tek "hero key visual" yetmiyor; art bible olmadan tutarlılık çöküyor ([nextmars](https://www.nextmars.com/post/nextmars-art-bible-cross-platform-visual-identity-20260331-0409)).

**Zorunlu bölümler** (her `games/<id>/art-bible.md` içinde):
1. **Style statement** — 1 paragraf + 3 referans görsel + 3 anahtar kelime (ör. "playful, bold, chunky").
2. **Color palette** — primary/secondary/accent + UI nötr + uyarı rengi. Her rengin hex + rol + yan yana kontrast testi.
3. **Silhouette rules** — karakter/obje thumbnailde tanınabilir mi? Sadece outline ile kimlik sınıfı okunabilmeli.
4. **Proportion bias** — character head-to-body ratio (chibi 1:2, casual 1:3, realistic 1:7).
5. **Line weight** — outline var mı, kaç px, constant mı tapered mı.
6. **Material treatment** — flat / cel-shaded / painterly / pixel. Seç ve kilitle.
7. **Lighting direction** — sabit yön + key/fill oranı.
8. **Environment motif** — tekrar eden form dili (yuvarlak köşe, 45°, vs).
9. **UI kit referansı** — frame sistemi, icon language, typography.
10. **Don't list** — 3–5 yasak (ör. "gradient shadow yok", "over-rendered detay yok").

Art bible template `templates/art-bible.md` olarak sisteme girmeli. Pixune'a göre flat style mobil casual segmentte baskın ([pixune](https://pixune.com/blog/game-art-styles/)).

---

## 2. Character design pipeline

Sabit akış (kısaltma yapma):
```
brief -> thumbnail (20-40 tiny) -> silhouette pass -> 3 color rough
  -> final front -> turnaround (front/3q/side/back) -> expression sheet (6-8)
  -> rig (Spine/Rive) -> sprite sheet OR skeletal export -> game import
```

**Thumbnail** 2–3 cm küçük, 20–40 adet, sadece silhouette netliği için ([artfolio](https://www.artfolio.com/article/character-turnaround-sheets-impress-recruiters-with-silhouette-clarity)). **Turnaround** minimum front + side + back; tercihen 3/4 de ([characterhub](https://characterhub.com/blog/character-resources/character-design-sheet)). **Expression sheet** animatöre emotion guide olarak verilir; 6–8 ifade: idle neutral, happy, angry, sad, surprised, hurt, victory, fail.

**2026 hızlandırıcısı**: CharacterGen tipi araçlar tek görselden turnaround + expression üretiyor ([charactergen](https://charactergen.app/character-design-sheet)) — konsept fazında prototipe uygun, final için manuel redraw gerekli.

---

## 3. Environment design

```
mood board (10-20 ref) -> thumbnail composition (5-8) -> perspective sketch
  -> value pass -> color pass -> final paint -> parallax layer split
```

**Parallax** için sahne **Foreground / Midground / Background / Static (sky)** olmak üzere 4 katmana bölünür. Yakın katman hızlı, uzak katman yavaş scroll eder — derinlik illüzyonu ([gamemaker](https://gamemaker.io/en/blog/creating-depth-and-immersion-parallax)). Hafif animasyon (yaprak, bulut, titreyen ışık) sahne hissini çoğaltır — tek layerli statik BG amatör durur ([300mind](https://300mind.studio/blog/game-background-design/)).

Mobilde düşük çözünürlük + performans testi şart; 2048×2048 üstü env texture yasak.

---

## 4. UI art system

**Icon language**: hepsi aynı grid (ör. 64px, 2px stroke, 4px corner radius). Among Us tarzı distinct icon = hızlı tanıma ([pixune UI](https://pixune.com/blog/best-examples-mobile-game-ui-design/)).

**Button system**: 3 state (idle/pressed/disabled) + 3 size (S/M/L) + 3 intent (primary/secondary/danger). 9-slice frame kullan → rescale pahasına bozulma yok.

**Frame system**: tek ortak "panel" frame + inner padding token (8/16/24).

**HUD**: sadece 3 şey göster — score, life/timer, pause. Aşırı bilgi yasak. UI çevre ile karışmalı (Sky: Children of the Light gibi blend) ([pixune UI](https://pixune.com/blog/best-examples-mobile-game-ui-design/)).

Referans: [Game UI Database](https://gameuidatabase.com/) — 55.000+ screenshot.

---

## 5. Animation teknolojileri

| Tip | Kullanım | Artı | Eksi |
|---|---|---|---|
| Frame-by-frame sprite | pixel art, low-frame karakter | basit, kontrol tam | atlas şişer |
| Sprite sheet atlas | casual karakter | GPU dostu, hızlı | bone reuse yok |
| Skeletal — **Spine** | ana karakter, boss | mesh deform, IK, physics | $379 pro ([slant](https://www.slant.co/versus/1900/15725/~spine_vs_dragonbones-pro)) |
| Skeletal — DragonBones (LoongBones) | bütçe sıfırsa | MIT runtime, free | tooling ölü ([slant](https://www.slant.co/versus/1900/15725/~spine_vs_dragonbones-pro)) |
| **Rive** | UI/icon, micro-interaction, state machine | interaktif state, .riv küçük, 60 FPS ([callstack](https://www.callstack.com/blog/lottie-vs-rive-optimizing-mobile-app-animation)) | karmaşık karakter için Spine kadar derin değil |
| Lottie | statik UI intro | tasarımcıdan direkt export | CPU rendering, React Native'de 17 FPS ([callstack](https://www.callstack.com/blog/lottie-vs-rive-optimizing-mobile-app-animation)); ThorVG iOS'ta %80 hızlandı |
| Live2D | anime-style 2.5D karakter | yüz nüansı | niche |

**MobileGameFactory tavsiyesi**: karakter → **Spine** (pro lisans alınacak); UI micro-interaction → **Rive**; loading/splash tek seferlik → Lottie kabul.

---

## 6. Sprite atlas & compression

- **Packing**: TexturePacker veya Unity SpriteAtlas; POT (power-of-two) zorunlu bazı engine'lerde — ASTC/PVRTC/ETC2 hizalaması için dimensions multiple-of-4 ([codeandweb](https://www.codeandweb.com/texturepacker/documentation/texture-settings)).
- **Compression**:
  - **ASTC** — Android %75+ destek, variable block size, ETC2'den kaliteli/küçük ([Android dev](https://developer.android.com/games/optimize/textures)). **Varsayılan seç.**
  - **ETC2** — fallback, eski cihaz uyumu.
  - **PVRTC** — eski iOS (artık opsiyonel).
- **Mipmap**: world sprite için açık (scaling artifacts), UI için kapalı (bellek tasarrufu).
- **Atlas boyutu**: max 2048×2048; karakter + env ayrı atlas; UI ayrı atlas.

---

## 7. Shader toolkit (mobil reflexler)

Küçük casual oyun için şunlardan fazlası lüks:
- **Cel shading** — 2-tone/3-tone lambert lookup, outline post-process veya inverted hull ([lettier](https://lettier.github.io/3d-game-shaders-for-beginners/cel-shading.html)).
- **Outline** — 1–2 px sabit kalın, siyah veya dark tone.
- **Glow/bloom** — selective, tek seferlik particle üzerinde.
- **Dissolve** — unlock/transition efekti.
- **Water** — scroll UV + foam alpha, tam dalga simülasyonu mobilde yasak.
- **Fire/smoke** — particle + additive blend, frame-animated sprite daha ucuz ([80.lv](https://80.lv/articles/vfx-and-shaders-fire-water-and-lasers)).

Mobilde post-process outline + 1–2 custom material sınırı. Tam PBR pipeline yasak.

---

## 8. AI asset generation — 2026 state

- **Midjourney v7** — concept art için baskın; v7 aesthetic + prompt adherence en yüksek ([akunhub](https://akunhub.com/review-midjourney-v7-2026-is-this-the-final-frontier-for-generative-art/)). Studios yoğun kullanıyor; ama style-consistency için fine-tune yok → sadece moodboard + early concept.
- **DALL-E 4** — OpenAI ekosisteminde, text integration kuvvetli; oyun art'ında ikincil.
- **Stable Diffusion 3 / SDXL + ControlNet** — endüstriyel kontrol (Canny/Depth/OpenPose) ile pose lock, pixel-level structural control ([legnext](https://legnext.ai/blog/midjourney-v7-vs-flux2-vs-sd3-the-2026-deep-dive-on-genai-model-selection)). Studios'ta standart.
- **Scenario** — custom model training, studio IP'sinde fine-tune. Production pipeline ([scenario](https://www.scenario.com/)).
- **Leonardo AI** — game asset category leader, 3D texture generation (albedo/normal/roughness) dahil ([leonardo](https://leonardo.ai/news/how-to-generate-a-full-game-asset-suite-with-leonardo-ai/)).

**Tavsiye pipeline**: konsept → Midjourney/Leonardo → iteration → SD+ControlNet ile pose/silhouette lock → Scenario custom model ile style consistency → manuel polish ve turnaround final. AI "85% starting point", %15 final manuel zorunlu.

---

## 9. 2D vs 2.5D vs 3D — casual mobile tradeoff

| Eksen | 2D | 2.5D | 3D |
|---|---|---|---|
| Art maliyet | düşük | orta | çok yüksek |
| Dev süresi | kısa | orta | uzun |
| Low-end cihaz | mükemmel | iyi | zor |
| APK boyut | küçük | orta | şişer |
| Casual fit | **en iyi** | iyi | opsiyonel |

3D AAA casual $500K–$1M+ ([kevuru](https://kevurugames.com/blog/differences-between-2d-games-vs-3d-games/)). MobileGameFactory hedefi **2D + seçici 2.5D (parallax, isometric görünüm)**. Tam 3D yasak.

---

## 10. Asset budget (per game, AAB ≤40 MB hedefli)

| Kategori | Hedef |
|---|---|
| Karakter atlas | ≤2× 2048² atlas |
| Environment atlas | ≤3× 2048² atlas |
| UI atlas | 1× 2048² |
| Toplam texture memory | ≤80 MB decompressed, ≤20 MB ASTC |
| Sprite sheet frame | karakter başı ≤40 frame (skeletal tercih) |
| Particle texture | ≤8 unique, 256² max |
| Font | ≤2 file, subset, ≤200 KB |
| Audio | 64–96 kbps OGG, music loop ≤500 KB, SFX ≤20 KB |
| Unique karakter | ≤5 (hero + 2 enemy + 2 NPC) |
| Unique env tile | ≤30 |
| UI icon | ≤40 |

---

## Pipeline diyagramı (oyun başına)

```
[brief.md] -> [market.md] -> [design.md]
        |
        v
[art-bible.md] -------> [moodboard/]
        |
  +-----+-----+---------+----------+
  v     v     v         v          v
[chars] [envs] [UI kit] [VFX]  [audio brief]
  |     |     |         |
  v     v     v         v
sprite/skeletal  parallax layers  9-slice  particle/shader
  \____________\________|_________/
                        v
                [asset atlas build]
                        v
              [games/<id>/src/.../Resources]
                        v
                 [QA visual regression]
```

## Pre-ship art checklist

- [ ] `art-bible.md` finalize + commit
- [ ] Palette kilitli + contrast test (WCAG AA icon readability)
- [ ] Character turnaround + 6 expression komplet
- [ ] Environment 3–4 parallax katman (PSD + export)
- [ ] UI kit: button 3-state × 3-size + frame 9-slice + ≥20 icon
- [ ] Atlas POT + ASTC build
- [ ] VFX: ≤8 particle + shader listesi
- [ ] Font subset + lisans dosyası
- [ ] Tüm asset `artifact_register` ile oyuna bağlandı
- [ ] 1× low-end Android'de frame-time ölçüldü

---

## Referans kaynakları

- [Art Bible Cross-Platform — nextmars](https://www.nextmars.com/post/nextmars-art-bible-cross-platform-visual-identity-20260331-0409)
- [Art Bible wiki — polycount](http://wiki.polycount.com/wiki/Art_Bible)
- [Game Art Styles 2026 — Pixune](https://pixune.com/blog/game-art-styles/)
- [Character Sheets 2026 — CG-Wire](https://blog.cg-wire.com/character-sheet-animation/)
- [Turnaround master — Artfolio](https://www.artfolio.com/article/character-turnaround-sheets-impress-recruiters-with-silhouette-clarity)
- [Spine vs DragonBones — Slant](https://www.slant.co/versus/1900/15725/~spine_vs_dragonbones-pro)
- [Lottie vs Rive — Callstack](https://www.callstack.com/blog/lottie-vs-rive-optimizing-mobile-app-animation)
- [Rive as Lottie alternative](https://rive.app/blog/rive-as-a-lottie-alternative)
- [TexturePacker settings](https://www.codeandweb.com/texturepacker/documentation/texture-settings)
- [Android textures ASTC/ETC2](https://developer.android.com/games/optimize/textures)
- [VFX & Shaders — 80.lv](https://80.lv/articles/vfx-and-shaders-fire-water-and-lasers)
- [Cel Shading beginners — lettier](https://lettier.github.io/3d-game-shaders-for-beginners/cel-shading.html)
- [Midjourney v7 vs Flux vs SD3 — legnext](https://legnext.ai/blog/midjourney-v7-vs-flux2-vs-sd3-the-2026-deep-dive-on-genai-model-selection)
- [Leonardo AI full game asset suite](https://leonardo.ai/news/how-to-generate-a-full-game-asset-suite-with-leonardo-ai/)
- [Scenario — production AI](https://www.scenario.com/)
- [Parallax — GameMaker](https://gamemaker.io/en/blog/creating-depth-and-immersion-parallax)
- [Background design — 300mind](https://300mind.studio/blog/game-background-design/)
- [Art asset budgeting mobile — Linden Reid](https://lindenreidblog.com/2022/05/27/optimization-strategies-for-mobile/)
- [Game UI Database](https://gameuidatabase.com/)
- [Mobile UI examples — Pixune](https://pixune.com/blog/best-examples-mobile-game-ui-design/)
