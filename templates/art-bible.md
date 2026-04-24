# Art Bible — {{title}}

**ID**: `{{id}}`
**Art Director**: {{name}}
**Tarih**: {{YYYY-MM-DD}}
**Kapı**: art-bible
**Versiyon**: 1.0 (kilitli — değişiklik için v2 fork)

---

## 1. Style statement
{{1 paragraf mood + 3 keyword}}

**Referans görseller** (ArtStation / Pinterest / Behance):
1. {{url}}
2. {{url}}
3. {{url}}

**Keyword**: {{kw1}}, {{kw2}}, {{kw3}}

---

## 2. Color palette
| Rol | Hex | Notu |
|---|---|---|
| Primary | #1F7A6C | Brand, menu dominant |
| Secondary | #F5A623 | Accent, HUD + primary button |
| Success | #4CAF50 | Quest complete |
| Warn | #FFC107 | Low energy |
| Error | #F44336 | Fail, crash |
| BG dark | #0E3B39 | Sayfa arka plan |
| BG light | #14524C | Kart / popup |
| Text primary | #FFFFFF | Headline |
| Text secondary | #C6E0C0 | Body |
| Text disabled | #7FA29B | Inactive |

**Contrast test**: WCAG AA (4.5:1 text, 3:1 UI). Deuteranopia + Protanopia + Tritanopia geçti.

---

## 3. Silhouette rules
- Her karakter 32px thumbnail'de ayırt edilebilir
- Hero profil yüksekliği min 28px
- İç detay minimize (okunurluk)

## 4. Proportion bias
- Karakter: chibi **1:2.5** head-to-body
- Pet: 1:1.8 (daha yuvarlak)

## 5. Line weight
- Outline **VAR**, 4px dark (#1B1B1B)
- İç line 2px

## 6. Material treatment
**Kilitli: cel-shaded cartoon** (2-3 tone, hard shadow)
- Yasak: painterly gradient, flat-no-shadow, pixel, 3D render

## 7. Lighting direction
- Top-left 45°
- Shadow: hard edge, 15% alpha
- No rim light

## 8. Environment motif dili
| World | Unique vocabulary |
|---|---|
| Tropik Orman | palm, water, stone, vines |
| Sahil Mağarası | shells, driftwood, sand, rope |
| Antik Tapınak | stone pillars, torches, rune, golden door |
| Volkan | lava crack, ember, basalt, steam |
| Buz Diyarı | icicle, snow, frozen ruin, aurora |

## 9. UI kit referansı
- **Frame style**: rounded rectangle 14-18 radius
- **Icon grid**: 24 / 32 / 48 px
- **Typography**:
  - Headline: Lilita One (display cartoon)
  - Body: Inter (legible)
- **Button**: pill shape, solid primary, soft shadow

## 10. Don't list
- Anatomik realism YOK
- Gradient glow YOK
- Neon saturation YOK
- Text-in-image YOK
- 3D render YOK

---

## Asset budget
- Karakter: ≤5 unique
- Environment tile: ≤30
- Icon: ≤40
- Toplam decompressed texture: ≤80 MB

## Pipeline
1. Concept: Midjourney v7 + Leonardo
2. Kontrol: SD XL + ControlNet
3. Style-lock: Scenario custom model (brand)
4. Polish: %15 manuel (silhouette + palette + bg removal)
5. Animation: Spine Pro (karakter) + Rive (UI micro)
6. Atlas: TexturePacker → ASTC compression
