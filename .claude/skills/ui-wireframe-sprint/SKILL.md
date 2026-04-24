---
name: ui-wireframe-sprint
description: UX/UI Designer'ın UX kapısında izlediği playbook. Menu + HUD + onboarding + popup sistemini wireframe olarak çıkarır.
---

# Skill: ui-wireframe-sprint

## Ne zaman
Art Bible + Design + Stage Plan sonrası. PM UX/UI Designer'ı çağırır.

## Ön koşul
- art-bible.md (UI kit ref), design.md, stage-plan.md okundu
- `.claude/rules/ux-principles.md` okundu
- MGF.UI kütüphanesi var (tools/MGF.UI/)

## Adımlar

### 1. Screen list çıkar
Minimum 10+ ekran:
- Splash
- Character Select (ilk kez)
- Main Menu
- Board (oyun)
- Biome / World Select
- Shop
- Settings
- Pause
- Level Complete
- Level Fail
- Daily Reward popup
- Event popup
- Starter Pack popup
- Credits (opsiyonel)

### 2. Navigation graph
ASCII veya Mermaid:
```
Splash → (first) CharacterSelect → MainMenu
MainMenu → Board → LevelComplete/LevelFail → MainMenu
MainMenu → BiomeSelect → Board
MainMenu → Shop
MainMenu → Settings
Board → Pause → MainMenu
```

### 3. HUD şeması (core gameplay)
≤5 element:
1. Currency + `+` (tap → shop)
2. Energy counter
3. Level / stage indicator
4. Notification rozet dot
5. Primary action button (pause / boost / ...)

Pozisyon + behavior her biri için.

### 4. Onboarding flow (60 saniye)
- 0-10s: splash + main menu → character select (ilk kez)
- 10-20s: stage 1 başla, pet dialog
- 20-40s: ilk merge → multi-channel feedback
- 40-55s: quest complete → reward chest
- 55-60s: stage 2 teaser + "Devam et"

İlk popup en erken oturum 2. İlk rewarded ad stage 4+. İlk interstitial stage 8+ (ilk 3 run yasak).

### 5. Popup queue rule
Oturum başına max 1 popup. Sıra:
1. Daily reward (oturum 1. tap'ta)
2. Event banner (aktif event varsa)
3. Starter pack (ilk 24h pencere)
4. Rate-me (oturum 7+ sonra)

### 6. Micro-interactions (zorunlu)
- Button press: scale 1.0 → 0.95 → 1.08 → 1.0 (120ms)
- Tab switch: underline slide 200ms
- Toast appear: fade + slide 250ms
- Popup open: scale 0.85 → 1.0 + fade 200ms
- Ripple tap: SKCanvasView circle expand 400ms
- Page transition: slide 300ms

### 7. Empty/loading/error/offline states
Her ekran için:
- Empty: icon + "Başla" CTA
- Loading: skeleton shimmer (spinner değil tek başına)
- Error: icon + mesaj + retry
- Offline: top banner 40px "İnternet yok"

### 8. Accessibility checklist
- Color contrast WCAG AA
- Color blind mode (Deuteranopia/Protanopia/Tritanopia test)
- Reduced motion toggle
- Text scale 100/115/130%
- Haptic opt-out
- Sound opt-out (music + SFX ayrı)
- Screen reader semantic

### 9. Localization readiness
- Text container %30 genişleme toleransı
- No text-in-image
- RTL support (FlowDirection)
- CJK/Arabic font fallback

### 10. ui-wireframe.md yaz
`games/<id>/ui-wireframe.md` — yukarıdaki 9 bölüm + ASCII wireframe diyagramları.
Uzunluk: 500-800 kelime.

## Kapanış
```
artifact_register(gameId, gate="ux", kind="notes", path="games/<id>/ui-wireframe.md")
message_send(to="project-manager", type="handoff", subject="ui-wireframe hazır", body="<screen + CTA + risk>")
log_append(agent="ux-ui-designer", gate="ux", gameId=<id>, decision="<HUD + popup>", why="<UX pattern>")
```

## Yasaklar
- Per-screen özel stil (MGF.UI bypass)
- Popup spam >1/oturum
- 3+ primary CTA
- Text-only onboarding (interactive step gerek)
- Fake loading bar

## Done
- ui-wireframe.md tam
- Navigation graph
- Accessibility checklist
- Handoff Asset Designer + Developer
- 1 log_append
