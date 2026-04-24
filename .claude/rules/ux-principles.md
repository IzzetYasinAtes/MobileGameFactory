# UX Prensipleri — Casual Mobile

## 5 sert kural
1. **60 saniye kuralı** — ilk dakikada: hesap/popup/reklam yok; bir başarı + bir teaser (Royal Match modeli)
2. **Tek primary CTA** — her ekran 1 primary buton (görsel ağırlık %100); ikincil %60 opaklık; 3+ primary buton yasak
3. **Popup queue** — oturum başına max 1 modal; sıra: daily → event → starter pack; skip her zaman mümkün
4. **HUD ≤5 öğe** — currency + `+`, energy, level, bildirim dot, primary action; timer görselse progress bar, sayısalsa ≥10s için
5. **Accessibility default-on** — color+pattern kombinasyonu, reduced motion toggle, text scale 100/115/130, sistem "Reduce Motion" takibi

## Onboarding flow (60 saniye)
- 0–10s: splash + main menu → CharacterSelectPage (ilk kez) veya direkt oyun
- 10–20s: stage 1 başla — pet dialog 1 cümle "İki taş birleştir!"
- 20–40s: oyuncu ilk merge → confetti + SFX + haptic (multi-channel feedback)
- 40–55s: quest complete → reward chest açılır → coin float to HUD
- 55–60s: Stage 2 teaser "Yeni mekanik: kristaller!" + CTA "Devam et"

Ardından:
- İlk popup (daily reward) en erken oturum 2
- İlk rewarded ad en erken stage 4 veya run 4
- İlk interstitial en erken stage 8 veya run 5 (ilk 3 run yasak)
- IAP starter pack en erken oturum 3 (24h pencere)

## Screen list (minimum)
1. Splash (0.5–1s)
2. Character Select (ilk kez)
3. Main Menu (hub)
4. Board (oyun)
5. Biome / World Select
6. Shop (IAP)
7. Settings
8. Pause (oyun içinde)
9. Level Complete
10. Level Fail
11. Daily Reward popup
12. Event popup (liveops)
13. Starter Pack popup
14. Settings / Accessibility
15. Credits (opsiyonel)

## Navigation pattern
- **Hub-centric** (Royal Match, Homescapes): main menu → direkt oyun
- **Map-centric** (Candy Crush): harita üzerinde stage select
- **Linear progression** (hyper-casual): stage complete → otomatik next

**Seçim kuralı**: live-ops ağırlıklı → hub; story-driven → map; hız odaklı → linear.

## Micro-interactions (zorunlu)
- **Button press**: scale 1.0 → 0.95 → 1.08 → 1.0 (120ms ease-out-back)
- **Tab switch**: underline slide 200ms cubic-out
- **Toast appear**: fade-in + slide-up 250ms
- **Popup open**: scale 0.85 → 1.0 + fade-in 200ms
- **Ripple on tap**: SKCanvasView ripple circle expand 400ms
- **Page transition**: slide left/right 300ms (forward) veya right/left (back)

## States (her ekran için)
- **Empty state**: "Henüz içerik yok" icon + 1 CTA ("Yeni başla")
- **Loading state**: skeleton shimmer (spinner tek başına değil)
- **Error state**: icon + friendly message + retry CTA
- **Offline state**: banner top 40px "İnternet yok, offline oynuyorsun"

## Accessibility (WCAG 2.2 AA + mobile specific)
- **Color contrast**: text/button WCAG AA (4.5:1 normal, 3:1 büyük text)
- **Color blind**: Deuteranopia simulasyonu test (Spectrum plugin)
- **Reduced motion**: setting toggle; on'da particle/shake/autoplay azalt
- **Text scale**: 100/115/130% (iOS/Android system scale takip et)
- **Haptic opt-out**: Settings'de toggle
- **Sound opt-out**: music + SFX ayrı slider 0–100
- **Screen reader**: Semantic.Description MAUI, icon'lara alt-text

## Localization readiness
- Text container **%30 genişleme toleransı** (TR → DE, AR uzar)
- No text-in-image (l10n değiştiremez)
- RTL support (Arabic) — FlowDirection `RightToLeft`
- Font fallback (CJK glyph için Noto Sans CJK, Arabic için Noto Naskh)

## Popup queue sistemi
```
Session start
  ↓
Check queue: [daily_reward, event_banner, starter_pack]
  ↓
Show top priority (1 at a time)
  ↓
User dismiss or claim
  ↓
Next session ← timer yeniden

IF oturum başına >1 popup → DESIGN FAIL
```

## Anti-patterns (YASAK)
- Reklam + popup aynı ekranda
- FTUE sırasında IAP popup
- Confirm dialog'unda "yes" daha prominent (bias)
- Settings'e ulaşmak için 3+ tap
- Cross-promo popup ilk oturumda
- Rate-me popup oturum 1-3'te (minimum 7+)
- Back button infinite loop (physical back her zaman çalışır)

## Jakob's UX laws (uygulama)
- **Law of Similarity**: benzer işlev = benzer görünüm
- **Hick's Law**: seçenek az, karar hızlı (max 5 primary action ekranda)
- **Fitts's Law**: primary button ≥56px (thumb reach)
- **Miller's Law**: HUD 7±2 öğe max (biz 5 hedefliyoruz)
- **Peak-End rule**: level complete (peak) + reward animation (end) polish'i %50 memnuniyet

## QA UX checklist
- [ ] 60 saniye FTUE geçilebilir
- [ ] Tüm ekran single-CTA kuralına uyuyor
- [ ] Popup queue çalışıyor (oturum başına 1)
- [ ] HUD ≤5 öğe
- [ ] Accessibility toggle aktif
- [ ] Reduced motion mode test edildi
- [ ] Color blind mode test edildi
- [ ] Text scale 130% görünümde container overflow yok
- [ ] RTL (Arabic) UI flip doğru
- [ ] Offline banner görünür
