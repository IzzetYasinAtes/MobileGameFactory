---
name: character-design-sprint
description: Asset Designer'ın karakter üretim sürecinde izlediği playbook. Concept → sprite → turnaround → sheet → rig.
---

# Skill: character-design-sprint

## Ne zaman
Art Bible + Design sonrası. Asset Designer yürütür.

## Ön koşul
- art-bible.md (style lock) + design.md okundu
- `.claude/rules/art-direction.md` okundu

## Adımlar

### 1. Brief toplama
Her karakter için:
- Name, role (protag/sidekick/pet/antagonist)
- Voice (art-bible karakter sheet)
- Visual anchor (key-art referansı varsa)
- Budget: ≤5 unique karakter toplam

### 2. Concept (Midjourney v7 + Leonardo)
- Broad ideation 5-10 varyant
- Seçilen varyant: style keyword art-bible uyumlu
- Seed kaydet (tutarlılık)

### 3. Silhouette test (32px)
- Thumbnail view 32px
- Ayırt edilebilir mi (diğer karakterlerden)?
- Düzelt: silhouette değiştir, renk değiştir

### 4. Color lock (palette)
- Art-bible palette'inden çek
- Primary + 2 secondary renk
- WCAG AA contrast

### 5. Final render (SD XL + ControlNet + Scenario)
- ControlNet ile pose lock (brief'teki portrait)
- Scenario custom model style-lock
- 512×512 transparent bg (rembg)
- Multi-pose: idle + happy + sad + celebrating (4 pose min)

### 6. Manuel polish (%15 zorunlu)
- Silhouette cleanup
- Palette snap (exact hex)
- Background removal kontrol
- Eyeline dengesi

### 7. Expression sheet (opsiyonel)
4-8 expression variant aynı base:
- idle, happy, sad, surprised, angry, thinking, celebrating, tired

### 8. Rig (Spine Pro)
Animator devralır. Asset Designer hazır sprite teslim eder.

### 9. Naming + resource path
- `games/<id>/assets/character_<name>.png` (underscore MAUI)
- `games/<id>/src/<id>/Resources/Images/character_<name>.png`
- Atlas'a dahil: `atlas_characters.png` (sonraki iterasyonda pack)

### 10. Asset test
- 32px silhouette OK
- Transparent bg
- Palette WCAG AA
- Size ≤500 KB
- Ship AAB'ye eklenince total ≤40 MB

## Kapanış
```
artifact_register(gameId, gate="asset", kind="asset", path="games/<id>/assets/character_<name>.png")
# her karakter için
message_send(to="project-manager", type="handoff", subject="karakter sprite hazır", body="<N karakter + quality>")
log_append(agent="asset-designer", gate="asset", gameId=<id>, decision="<characters + style>", why="<art-bible uyumu>")
```

## Yasaklar
- Style drift (art-bible dışı)
- Silhouette test fail
- Budget aşımı (>5 karakter)
- Hyphen filename
- Bg removal atlama (karakter için transparent zorunlu)

## Done
- Her karakter: concept + silhouette + final + polish
- Transparent bg
- Resources/Images'a kopyalandı
- 1 log_append
