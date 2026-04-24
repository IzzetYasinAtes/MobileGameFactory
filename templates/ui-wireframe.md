# UI Wireframe — {{title}}

**ID**: `{{id}}`
**UX/UI Designer**: {{name}}
**Tarih**: {{YYYY-MM-DD}}
**Kapı**: ux

---

## 1. Screen list
| ID | Ekran | Amaç |
|---|---|---|
| 1 | Splash | 0.5-1s brand |
| 2 | CharacterSelect | İlk açılış, karakter seç |
| 3 | MainMenu | Hub — Oyna CTA |
| 4 | Board | Core gameplay |
| 5 | BiomeSelect | World seç |
| 6 | Shop | IAP + Remove Ads |
| 7 | Settings | ses, haptic, dil, accessibility |
| 8 | Pause | Oyun sırasında |
| 9 | LevelComplete | Reward + next |
| 10 | LevelFail | Retry + skip |
| 11 | DailyReward | Popup oturum 1 |
| 12 | EventPopup | Aktif event |
| 13 | StarterPack | İlk 24h |
| 14 | Credits | Opsiyonel |

## 2. Navigation graph

```
Splash → (first) CharacterSelect → MainMenu
MainMenu → Board → LevelComplete/LevelFail → MainMenu
MainMenu → BiomeSelect → Board
MainMenu → Shop
MainMenu → Settings
Board → Pause → MainMenu
```

## 3. HUD şeması (core gameplay)

```
┌─────────────────────────────────┐
│ [Coin+][Energy] │ [Level] [🔔] │  ← HUD top (4 element)
├─────────────────────────────────┤
│                                 │
│         GAME BOARD              │
│         (7x9 grid)              │
│                                 │
├─────────────────────────────────┤
│        [Primary CTA]            │  ← 5. element (action)
└─────────────────────────────────┘
```

5 HUD element:
1. Coin counter + "+" (tap → Shop)
2. Energy counter
3. Level indicator
4. Notification rozet (daily/event dot)
5. Primary action button (Pause)

## 4. Onboarding flow (60 saniye)

```
0s    Splash (brand)
2s    → CharacterSelect (ilk kez)
8s    → MainMenu (logo + pet + Oyna)
12s   → Board (stage 1-01)
13s   Pet dialog: "İki taş birleştir!"
20s   Player tap T1 → select
22s   Player tap T1 → MERGE
       - particle burst + color flash + SFX + haptic + shake + hit stop
25s   Quest complete → chest float
30s   Reward coin → HUD float
40s   Stage 2 teaser
55s   "Devam et" CTA
60s   → Stage 2
```

**İlk popup**: oturum 2+ (daily reward)
**İlk rewarded**: stage 4+
**İlk interstitial**: stage 8+ (ilk 3 run yasak)
**IAP starter**: oturum 3+ (24h pencere)

## 5. Popup queue rule
Oturum başına max 1. Sıra:
1. Daily reward (ilk tap)
2. Event banner (aktif event varsa)
3. Starter pack (ilk 24h)
4. Rate-me (oturum 7+)

## 6. Micro-interactions
- **Button press**: scale 1.0 → 0.95 → 1.08 → 1.0 (120ms ease-out-back)
- **Tab switch**: underline slide 200ms cubic-out
- **Toast**: fade + slide-up 250ms
- **Popup open**: scale 0.85 → 1.0 + fade 200ms
- **Ripple**: SKCanvasView circle expand 400ms
- **Page transition**: slide 300ms forward (right-to-left; RTL için flip)

## 7. States (her ekran için)
- **Empty**: "Henüz içerik yok" icon + CTA "Başla"
- **Loading**: skeleton shimmer
- **Error**: icon + mesaj + retry
- **Offline**: top banner 40px "İnternet yok, offline oynuyorsun"

## 8. Accessibility checklist
- [ ] WCAG AA contrast
- [ ] Color blind mode (Deuteranopia + Protanopia + Tritanopia)
- [ ] Reduced motion toggle (particle/shake azalt)
- [ ] Text scale 100/115/130%
- [ ] Haptic opt-out
- [ ] Sound opt-out (music + SFX ayrı slider)
- [ ] Screen reader semantic

## 9. Localization readiness
- Text container %30 genişleme toleransı
- No text-in-image
- RTL (Arabic) FlowDirection="RightToLeft"
- CJK/Arabic font fallback (Noto family)

## MGF.UI kullanım
- PrimaryButton, CurrencyPill, RewardModal, ToastService, PopupQueue, AccessibilityPrefs
- Oyun-özel tema override: `Resources/Styles/<id>-theme.xaml`
