---
name: art-bible-brief
description: Art Director'ın Art Bible kapısında izlediği playbook. Brief + Market'ten hareketle art-bible.md üretir. 10 zorunlu öğe + referans link.
---

# Skill: art-bible-brief

## Ne zaman
Research sonrası, Design öncesi. PM Art Director'ı çağırır.

## Ön koşul
- brief.md (+ referans key-art varsa `assets/brand-keyart.png`)
- market.md (rakip visual teardown)
- `.claude/rules/art-direction.md` okundu
- `templates/art-bible.md` hazır

## Adımlar

### 1. Referans + mood analizi
- brief'teki key-art'ı incele
- market.md'deki 3 rakip'in visual özeti
- 3 referans görsel topla: ArtStation/Pinterest/Behance link

### 2. Style seçimi
Tek stil seç (değişmez):
- **Flat minimalist**: hyper-casual, puzzle
- **Cel-shaded cartoon**: casual adventure, match-3 (MGF default)
- **Painterly**: premium puzzle, narrative
- **Pixel art**: retro, roguelite

### 3. 10 zorunlu öğe

#### 1. Style statement
1 paragraf mood + 3 referans link + 3 keyword.
Örnek: "Sıcak tropik palet, stylized cartoon. Royal Match + Homescapes rafında ama Türk kültürel motifler. Warm, cozy, adventure."

#### 2. Color palette (WCAG AA)
6-12 hex, rol atanmış:
- Primary: brand rengi
- Secondary: accent
- Success / Warn / Error
- Neutral bg, border
- Text primary / secondary / disabled
Contrast testi: Deuteranopia + Protanopia + Tritanopia simulate.

#### 3. Silhouette rules
Thumbnail 32px'te ayırt edilebilir. Karakter profil yüksekliği min 28px.

#### 4. Proportion bias
Head-to-body ratio:
- Chibi 1:2.5 (casual cartoon)
- Semi-realistic 1:5
- Realistic 1:7

#### 5. Line weight
- Outline var / yok
- Varsa kaç px (mobile 3-5px dark önerilir)

#### 6. Material treatment
Kilitli seçim:
- Flat (no shade)
- Cel-shaded (2-3 tone)
- Painterly (soft gradient)
- Pixel (hard pixel, 1 color per block)

#### 7. Lighting direction
- Top-left 45° default
- Shadow soft vs hard
- Rim light var mı?

#### 8. Environment motif dili
Her biome için obje vocabulary:
- Tropik orman: palm + water + stone
- Volkan: lava + crack + ember
- Buz: icicle + snow + cave

#### 9. UI kit referansı
- Frame style
- Icon grid 24/32/48
- Typography 2 font max (headline + body)

#### 10. Don't list
3-5 yasak:
- Anatomik realism yok
- Gradient glow yok
- Neon saturation yok
- Text-in-image yok
- 3D render yok

### 4. Asset budget hatırlatma
- ≤5 unique karakter
- ≤30 env tile
- ≤40 icon
- ≤80 MB decompressed texture

### 5. art-bible.md yaz
`games/<id>/art-bible.md` — `templates/art-bible.md` kopyalanıp doldurulur.
Uzunluk: 400-700 kelime + görsel link tablosu.

## Kapanış
```
artifact_register(gameId, gate="art-bible", kind="notes", path="games/<id>/art-bible.md")
message_send(to="project-manager", type="handoff", subject="art-bible hazır", body="<style + palette + 1 risk>")
log_append(agent="art-director", gate="art-bible", gameId=<id>, decision="<style + palette>", why="<rakip + brief uyumu>")
```

## Yasaklar
- "Her stile uyar" ifadesi
- Referans görsel olmadan style statement
- Don't list eksik
- Asset budget dışı öneri

## Done
- art-bible.md 10 öğe tam
- 3 referans görsel link
- Palette WCAG AA
- PM handoff + 1 log
