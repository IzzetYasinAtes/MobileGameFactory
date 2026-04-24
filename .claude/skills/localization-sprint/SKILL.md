---
name: localization-sprint
description: Localization agent'ın Localization kapısında izlediği playbook. TR + EN base + Tier 2+ planning.
---

# Skill: localization-sprint

## Ne zaman
Release öncesi. Paralel: LiveOps + Analytics + Growth.

## Ön koşul
- narrative.md, ui-wireframe.md, design.md okundu
- `.claude/rules/localization.md` okundu

## Adımlar

### 1. String extract
MAUI AppResources.resx → `l10n/strings.en.json` (base English).

Key convention:
- `ui.btn.<name>`
- `ui.label.<name>`
- `ui.popup.<id>.title/body/cta`
- `gameplay.quest.<stage_id>.intro/outro/hint1/hint2`
- `char.<name>.bark.<type>_<num>`
- `error.<type>`
- `store.<sku>.name/desc`

### 2. TR base translate
Bulk GPT-4 veya Claude ile strings.en.json → strings.tr.json. Native review (sahibin Türkçe bilgisi = self-review).

### 3. Glossary üret
`l10n/glossary.md` terminology sabitleri:
- Stone = Taş
- Wood = Odun
- Crystal = Kristal
- Merge = Birleştir
- Quest = Görev
- Boost = Güçlendirici

**Kilitli** — değişirse tüm re-translate.

### 4. Tier planning
- **v1.0 zorunlu**: TR + EN
- **v1.0 +30 gün**: DE, FR, ES, PT-BR (Tier 2)
- **v1.0 +60 gün**: AR, RU
- **v1.0 +90 gün**: JA, KO, ZH-CN

### 5. Kültürel adaptasyon notları
`l10n/cultural-notes.md`:
- Ramazan TR için
- Sakura JP
- Diwali IN
- Culture-specific font fallback
- RTL (AR) UI flip

### 6. Pseudo-locale testing
`xx-pseudo.json` — string %30 genişletilmiş. UI container overflow test.

### 7. RTL support (Arabic tier için)
- MAUI `FlowDirection="RightToLeft"` page-level
- Icon arrow flip
- Progression map right-to-left

### 8. Font fallback
- Latin: Lilita One + Inter
- CJK: Noto Sans CJK (JP/KR/TC/SC)
- Arabic: Noto Naskh / Cairo
- Cyrillic: Noto Sans Cyrillic

### 9. Store metadata localization
Her pazar için (min TR + EN):
- Title (30 char)
- Short description (80 char)
- Long description (4000 char)
- Screenshot l10n overlay
- Video preview sub-caption

### 10. QA
- Hardcoded string taraması (grep → 0 sonuç)
- Pseudo-loc test overflow yok
- RTL flip doğru
- Cultural insensitivity check

## Kapanış
```
artifact_register(gameId, gate="localization", kind="notes", path="games/<id>/l10n/")
message_send(to="project-manager", type="handoff", subject="l10n TR+EN hazır", body="<string count + locale>")
log_append(agent="localization", gate="localization", gameId=<id>, decision="<locales + glossary>", why="<pazar uyumu>")
```

## Yasaklar
- Google Translate tek kaynak Tier 2+
- Hardcoded string
- Text-in-image
- Cultural insensitivity
- RTL skip
- Gender stereotyping

## Done
- TR + EN string bundle
- Glossary.md
- Pseudo-loc test
- 1 log_append
