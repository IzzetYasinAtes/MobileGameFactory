---
name: environment-design-sprint
description: Asset Designer'ın environment (biome/world) üretim playbook'u. Mood board → sketch → paint → parallax.
---

# Skill: environment-design-sprint

## Ne zaman
Art Bible + Stage Plan sonrası. Asset Designer yürütür.

## Ön koşul
- art-bible.md (environment motif dili) + stage-plan.md (world listesi) okundu
- `.claude/rules/art-direction.md` okundu

## Adımlar

### 1. World listesi
stage-plan.md'den al. Her world için:
- Tema (tropik orman, sahil mağarası, antik tapınak, volkan, buz)
- Palette (art-bible environment motif)
- Unique props vocabulary

### 2. Mood board (Midjourney v7)
Her world için 3-5 broad concept. Style keyword art-bible uyumlu.

### 3. Sketch + paint (SD XL)
- Composition: foreground + midground + background
- Perspective: 3/4 veya top-down (mobile portrait UI-friendly)
- Palette uygulandı

### 4. Parallax layers (2.5D için)
Her environment 2-3 katman:
- Background (distant, slow scroll)
- Midground (mid scroll)
- Foreground (fast scroll)
Optional: UI overlay katman

### 5. Compression + budget
- Atlas: env_<world_name>.png, 1024×1024 veya 2048×2048 POT
- ASTC 6x6 block compression
- Budget: ≤30 unique tile
- Decompressed ≤30 MB

### 6. Naming + path
- `games/<id>/assets/env_<name>.png`
- `games/<id>/src/<id>/Resources/Images/env_<name>.png`

### 7. Asset test
- Palette uyumlu (WCAG AA)
- Silhouette (hero element ayırt edilebilir)
- Size uygun
- Tile seamless (repeat pattern için)

## Kapanış
```
artifact_register(gameId, gate="asset", kind="asset", path="games/<id>/assets/env_<name>.png")
# her environment için
message_send(to="project-manager", type="handoff", subject="env asset hazır", body="<N env + total size>")
log_append(agent="asset-designer", gate="asset", gameId=<id>, decision="<environments>", why="<palette uyumu>")
```

## Yasaklar
- Style drift
- Palette uyumsuz
- Budget aşımı (>30 tile)
- Hyphen filename
- 3D render (2D/2.5D only)

## Done
- Her world environment sprite var
- Resources/Images'a kopyalandı
- Budget ≤80 MB toplam
- 1 log_append
