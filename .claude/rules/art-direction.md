# Art Direction Kuralları

## Sert kurallar
1. **Art bible zorunlu** — Art Director tarafından yazılır, tek source-of-truth
2. **Stil kilitli** — seçildikten sonra oyun boyunca değişmez (flat / cel / painterly / pixel)
3. **2D baskın** — 3D yasak (AAB boyut + performans); 2.5D sadece parallax veya isometric
4. **Asset budget bağlayıcı** — ≤5 unique karakter, ≤30 env tile, ≤40 icon, ≤80 MB decompressed texture
5. **Silhouette testi** — her karakter/obje 32px thumbnail'de ayırt edilebilir olmalı

## Stil seçim matrisi

| Stil | Ne zaman | Örnek oyun |
|---|---|---|
| **Flat minimalist** | Hyper-casual, puzzle, idle | Two Dots, Monument Valley |
| **Cel-shaded cartoon** | Casual adventure, match-3 | Royal Match, Homescapes |
| **Painterly** | Premium puzzle, narrative | Alto's Adventure, Journey |
| **Pixel art** | Retro, roguelite, arcade | Monument Valley 2, Stardew |
| **Anime / manga** | Story-rich RPG, gacha | Genshin, Fate Grand Order |

**MGF default**: casual merge/match-3 için **cel-shaded cartoon** — warmth + clarity.

## Art bible zorunlu 10 öğe
1. **Style statement** (1 paragraf + 3 referans link + 3 keyword)
2. **Color palette** (6–12 hex, rol atanmış, WCAG AA kontrast)
3. **Silhouette rules** (thumbnail okunurluk)
4. **Proportion bias** (head-to-body ratio)
5. **Line weight** (outline var/yok, kaç px)
6. **Material treatment** (flat/cel/painterly/pixel — KİLİTLİ)
7. **Lighting direction** (top-left 45° default; shadow soft/hard)
8. **Environment motif dili** (obje vocabulary her biome için)
9. **UI kit referansı** (frame, icon grid 24/32/48, typography 2 font max)
10. **Don't list** (3–5 yasak)

## Palette kuralları
- **Primary**: brand rengi (menüde dominant)
- **Secondary**: accent (HUD + primary button)
- **Success / Warn / Error**: sistemik (green/yellow/red)
- **Neutral**: bg, border, disabled
- **Text**: primary + secondary + disabled (3 kontrast seviye)
- **WCAG AA**: 4.5:1 text, 3:1 UI component contrast
- **Color blind safe**: Deuteranopia + Protanopia + Tritanopia simulate

## Iconography
- **Grid**: 24 / 32 / 48 px (mobile), 64 / 128 (hero)
- **Style**: solid fill (flat), soft shadow, 2–3 color max per icon
- **Font icons**: Material Icons, FontAwesome — ok, AMA game-specific icon zorunlu (brand)
- **Resolution**: @1x, @2x, @3x Android density buckets veya SVG

## Typography
- **Max 2 font** — primary (headline) + secondary (body)
- **Primary**: display font (Lilita One, Bungee, Pochayevsk — cartoon casual)
- **Secondary**: UI font (Noto Sans, Inter, SF Pro — legibility)
- **Size scale**: 10 / 12 / 14 / 16 / 20 / 24 / 32 px
- **Line height**: 1.4× font size
- **Weight**: regular + bold (2 max)
- **Fallback**: CJK, Arabic için Noto family

## Animation pipeline
- **Character skeletal**: Spine Pro ($379/seat; karakter animasyonu de-facto)
- **UI micro-interaction**: Rive ($25/ay starter; button, toast, tab)
- **Splash / intro**: Lottie (After Effects export; sadece splash)
- **Frame-by-frame**: 2D pixel art için; 4–8 frame walk cycle
- **Sprite sheet**: atlas 2048×2048 POT, ASTC compression

## Atlas & compression
- **Format**: POT (power-of-two) 256/512/1024/2048
- **Compression**: **ASTC 6×6 block** (iOS + modern Android); fallback **ETC2** (older Android)
- **Mipmap**: sadece dünya map (zoom out ölçek) için, UI değil
- **Atlas tool**: TexturePacker (Unity/Godot), MAUI manuel grid pack
- **Budget**: 1 oyun max 4 atlas (karakter + env + UI + FX), toplam ≤80 MB

## AI asset generation 2026 state

| Tool | Use case | Quality | Lisans |
|---|---|---|---|
| Midjourney v7 | Concept art, broad ideation | ⭐⭐⭐⭐⭐ | $30/ay Pro, ticari OK |
| Leonardo | Style variants, character turnaround | ⭐⭐⭐⭐ | $24/ay Artisan |
| Stable Diffusion XL + ControlNet | Pose/composition lock | ⭐⭐⭐⭐ | Ücretsiz, yerel |
| Scenario.gg | Custom trained model (brand style lock) | ⭐⭐⭐⭐⭐ | $30/ay Creator |
| Meshy / Tripo | 3D model gen | ⭐⭐⭐ | $20/ay |
| Rive | UI animation | N/A | $25/ay |

**Pipeline önerisi**:
1. Concept: Midjourney v7 + Leonardo (broad)
2. Kontrol: SD XL + ControlNet (pose lock)
3. Style-lock: Scenario custom model
4. **%15 manuel polish zorunlu**: silhouette cleanup, palette snap, bg removal (rembg)
5. Atlas pack: TexturePacker

## Asset QA checklist
- [ ] Silhouette test 32px geçti
- [ ] Palette WCAG AA
- [ ] Atlas ≤2048 POT
- [ ] Compression ASTC/ETC2 uygulandı
- [ ] Transparent bg (karakter için)
- [ ] Naming convention: `character_<name>.png`, `env_<name>.png`, `icon_<name>.png` (MAUI underscore kuralı)
- [ ] Budget aşılmadı (karakter ≤5, env ≤30, icon ≤40)
- [ ] Art bible don't list'ine uyuyor

## Anti-patterns (YASAK)
- Stil drift (aynı oyunda 2 farklı stil karışık)
- Gradient overload (her yüzey gradient → flat style bozulur)
- Text-in-image (l10n değiştiremez)
- Stock asset (unique brand identity kırılır)
- High-poly 3D (mobil hedef değil)
- Neon glow spam (chromatic aberration eyestrain)
