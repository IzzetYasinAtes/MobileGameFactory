---
name: aso-sprint
description: Growth Marketing'in ASO kapısında izlediği playbook. Title + description + keyword + screenshot + video strategy.
---

# Skill: aso-sprint

## Ne zaman
Release öncesi, LiveOps ile paralel. Growth Marketing yürütür.

## Ön koşul
- design.md, market.md, monetization.md okundu
- `.claude/rules/growth.md` okundu

## Adımlar

### 1. Title
- Play Store: 30 char, tam 50 (ilk 30 görünür)
- App Store: 30 char
- Format: brand + 1-2 keyword
- Örnek: "Mini Kaşifler: Ada Macerası" (27 char)

### 2. Short description (Play Store, 80 char)
Oyun tipi + value prop + CTA.
Örnek: "Merge, keşfet, çöz! Gizemli adada eğlenceli macera seni bekliyor. Oyna!"

### 3. Long description (4000 char)
Yapı:
- Hook paragraf (3 satır, emoji opsiyonel)
- Özellik listesi (6-10 bullet)
- Sosyal kanıt (rating, download, press)
- What's new (son sürüm)
- Gizlilik + support link

### 4. Keyword pool
- Primary 5 (genre + core mechanic)
- Secondary 10 (brand + theme + feature)
- Long-tail 15 (niche + localized)

### 5. Her pazar için TR + EN + DE + FR + ES + AR + JA + ZH-CN ayrı optimize
Native keyword research: Sensor Tower, AppMagic.

### 6. Icon (512×512 Play, 1024×1024 App Store)
- A/B test 2 varyant (hero character vs emblem)
- Silhouette 48px okunur
- Brand ana renk %60+
- Art bible ile tutarlı

### 7. Screenshots (5-8)
- Mobile portrait (1080×1920 Play, 1242×2688 App Store)
- A/B test 2 set ("gameplay" vs "feature")
- İlk 3 screenshot kritik
- Localized text overlay (kısa, 3-5 kelime)

### 8. Feature graphic (1024×500)
2 varyant A/B. Hero + tagline.

### 9. Video preview (15-30s)
- Landscape (Play) + Portrait (App Store)
- Hook ilk 3 sn
- Gameplay %70+
- Sub-caption (mute default)
- CTA sonunda

### 10. ASO checklist
- [ ] TR + EN metadata ready
- [ ] Icon 2 varyant
- [ ] 5+ screenshot her dil
- [ ] Feature graphic 2 varyant
- [ ] Video preview
- [ ] Keyword density 1-2% (long description)

## Kapanış
```
artifact_register(gameId, gate="growth", kind="notes", path="games/<id>/aso.md")
message_send(to="project-manager", type="handoff", subject="ASO hazır", body="<keyword + asset count>")
log_append(agent="growth-marketing", gate="growth", gameId=<id>, decision="<keyword strategy>", why="<market uyumu>")
```

## Yasaklar
- Keyword stuffing
- Misleading screenshot (oyunda olmayan)
- Fake review satın alma
- Cross-promo kid-directed (Families Policy)

## Done
- ASO TR + EN hazır
- Icon + screenshot + video
- Keyword pool test edilmiş
- 1 log_append
