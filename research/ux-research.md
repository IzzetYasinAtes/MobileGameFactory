# Casual Mobile Game UX/UI Research

Tarih: 2026-04-23 | Kapsam: .NET MAUI tabanlı küçük-orta ölçekli casual oyunlar için yeniden kullanılabilir UX/UI sistem önerileri. Referanslar dip notta.

---

## 1. Onboarding — 60 saniye kuralı
**Prensip.** İlk 60 saniye içinde oyuncu (a) ana mekanik ile bir başarı yaşar, (b) neden devam edeceğine dair bir sebep görür. İlk 30 dakika retention'ı belirler; ilk dakika "stay vs churn" kararını verdirir.

**Somut örnek.** Royal Match: splash → karakter (King Robert) → ilk level "forced-win" → yıldız + coin patlaması → castle renovation teaser. Hesap yok, popup yok, reklam yok. Tüm akış <60 s.

**Anti-pattern.** Hesap oluşturma ekranı, 3 slide'lık pasif tutorial, ilk gate'te IAP banner. Playrix/Homescapes erken dönem de dahil buna yaklaşan çoğu oyun D1 retention kaybı yaşadı.

**Kural.** Tutorial interaktif; pasif metin ≤ 1 ekran. Zorunlu hesap yasak. İlk 2 run boyunca IAP ve interstitial ad bloklu (bkz. `monetization.md`).

## 2. Main menu / hub pattern'ı
**Prensip.** Tek hero illustration + 1 primary CTA ("Play") + ≤3 ikincil eylem (shop, daily, settings). Minimal chrome, enerjik renk, net vurgu.

**Somut örnek.** Royal Match: castle arka plan canlı (NPC yürür, kuşlar uçar), "Play" butonu pulsing spring animasyonu ile nefes alır. Homescapes: mansion room hub aynı zamanda progress metaforu. Candy Crush: saga map kendisi hub.

**Anti-pattern.** 8+ buton kaplı ana menü (hyper-casual stilini casual'a yapıştırmak), tüm butonlar aynı vurguda → oyuncu nereye basacağını bilmez.

**Kural.** Ana menüde tek primary CTA; ikincil eylemler %60 opaklıkta. Hero alanı ekranın ≥ %50'si.

## 3. HUD standartları
**Prensip.** HUD corner-based: sol üst = identity/level, sağ üst = currency + "+" shortcut, alt orta = primary action (opsiyonel), köşelerde rozet. Oyun boyunca **≤5** sürekli görünür öğe.

**Somut örnek.** Currency pill (ikon + sayı + "+") sağ üstte. "+" dokunuşu direct shop modal → currency packs. Red dot (rozet) daily reward + mailbox + shop update için; sayı vermek yerine sadece nokta (sayı verince baskı psikolojisi casuaal'da ters teper).

**Anti-pattern.** Timer'ı saniye cinsinden sayısal yazmak ("04:37") yerine progress bar daha az stresli. Ekranın tamamını kaplayan notification kuyruğu. Banner ad gameplay ekranında (yasak, bkz. `monetization.md`).

**Kural.** Currency her zaman görünür + `+` shortcut. Rozet sayıyla değil dot ile. Timer görselse progress bar; sayısalsa ≥10 s için.

## 4. Dialog / toast / modal / popup
**Prensip.** Hiyerarşi: **toast** (≤3s, non-blocking, alt) → **snackbar** (action'lı toast) → **dialog** (onay, blocking, ortada) → **full-screen modal** (shop, daily reward, major event).

**Somut örnek.** "2x coins earned!" → toast. "Spend 500 coins to continue?" → dialog (iki buton: Yes/No eşit değerde değil — Yes primary, No ghost). "Daily rewards" → full-screen modal, sadece session başında 1 kez.

**Anti-pattern.** Popup queue (aynı anda 3 popup arkaya stack). Close (X) butonu 24dp'den küçük. Geri tuşu modal'ı kapatmıyor (Android'de MAUI `BackButtonBehavior` ile kapatılmalı).

**Kural.** Tek seferde 1 popup. Close ≥44dp (HIG) / 48dp (Material). Focus trap içinde. Tap-outside modal'ı kapatır (destructive değilse).

## 5. Navigation — tab bar vs hub vs side menu
**Prensip.** Küçük casual oyun = hub-centric. Tab bar orta ölçek (3-5 bölüm) için. Side menu (hamburger) casual'da **yasak** — keşfi öldürür.

**Somut örnek.** Royal Match: hub-only (levels + coin shop erişimi ufuk-çizgi). Clash Royale: alt tab bar (5 tab). Homescapes: hub = story ilerleme + match-3 giriş butonu üstte.

**Anti-pattern.** Hamburger menü altında gizli daily reward → oyuncu bulamaz → retention düşer. 7+ tab bar → tıklama ıskalaması artar.

**Kural.** Küçük oyun: hub pattern. Daily reward, shop, settings hub'dan 1 dokunuşta.

## 6. Micro-interactions — juice
**Prensip.** Her dokunuş <100 ms görsel yanıt, ≤300 ms transition. Squish-on-press (button scale 1.0 → 0.92 → 1.0), ripple (Material), spring release.

**Somut örnek.** Royal Match: coin collect → parçacıklar + counter tick-up ses + haptic "light". Button press: scale bounce + hafif ses + ripple. Match combo: screen shake (küçük, 2-3 px), particle burst, chain ses yükselen pitch.

**Anti-pattern.** Frame rate düşerken lottie animasyon kasar (bkz. `performance.md`). Ses geç gelir (>150 ms lag) → hissizleşir. Her butonda screen-shake → migren.

**Kural.** Touch-down feedback <100 ms. MAUI'de `GraphicsView`/SkiaSharp ile custom spring; aşırı Lottie yasak.

## 7. Empty / loading / error / offline state
**Prensip.** Hiçbir ekran tamamen "boş" olmaz. Her state (a) durumu açıkla, (b) aksiyon öner, (c) ton ve marka ile uyumlu illustration.

**Somut örnek.** Empty inventory: "Kasan bomboş!" + shop'a git CTA. Loading: skeleton/shimmer değil, karakter animasyonu + ipucu metni ("Tip: combo'lar 3x puan verir"). Error: "Bir şey ters gitti. Tekrar dene." + retry. Offline: "İnternet yok — offline oynayabilirsin" (yerel-only olduğumuz için iyi taraftır).

**Anti-pattern.** Sonsuz spinner. "Error 403" gibi teknik kod. Offline'da uygulama çöker.

**Kural.** Her Page `OnAppearing`'de loading skeleton; timeout 10 s sonra error state. Offline fallback daima var (yerel-only mimari avantajı).

## 8. Accessibility
**Prensip.** Color-blind, motion-sensitive, düşük görüşlü oyuncular için opsiyon default olarak erişilebilir.

**Somut örnek.** Color-blind mode: renk + şekil/desen kombinasyonu (eşleştirme oyunlarında pattern overlay). Reduced motion: screen shake + particle %50 kırpma. Text scale: 100/115/130 % tercih. Haptic on/off.

**Anti-pattern.** Sadece renk kodlu bilgi (kırmızı = tehlike, yeşil = güvenli) — %8 erkek kullanıcı ayırt edemez. Kapatılamayan motion efektleri.

**Kural.** Settings → Accessibility alt menüsü: color-blind preset, reduced motion, text scale, haptic. iOS "Reduce Motion" sistem ayarı takip edilmeli.

## 9. Dark mode / light mode
**Prensip.** Casual oyunlar çoğunlukla **painted** stil — full dark mode shift gereksiz. Sistem ayarına duyarlı olmak yerine **kalıcı marka palette** + HUD/menu karanlık varyantları yeterli.

**Somut örnek.** Royal Match: her zaman ışıklı-renkli (ürünün kendisi bir "dark mode"). Settings ekranı varsayılan açık tema; OLED yakmasını engellemek için gece lobbisi tonu %10 koyulaştırılır.

**Anti-pattern.** Tüm asset'leri iki set render edip boyutu 2x yapmak (AAB ≤40 MB kuralı ihlali, bkz. `performance.md`).

**Kural.** Tam dark mode yerine "night lobby" toggle; oyun içi painted palette sabit.

## 10. Casual vs mid-core vs hyper-casual UI farkı
| Boyut | Hyper-casual | Casual | Mid-core |
|---|---|---|---|
| Tutorial | 3-5 s, tek gesture | Interaktif 60 s | 5-15 dk scripted |
| HUD öğe sayısı | 1-2 | 3-5 | 7+ |
| Ana menü | Play only | Hub + shop + daily | Multi-tab, sosyal |
| Renk | Flat, minimal | Painted, doygun | Detailed 3D/PBR |
| Font | Sans-serif rounded | Custom painted display | Serif/heavy |
| Para kazanma | Interstitial-heavy | Rewarded + soft IAP | IAP-heavy, battle-pass |

Kendimizi **Casual** dilimine konumlandırıyoruz; hyper-casual hızı + casual polish.

## 11. Font / color / iconography
**Prensip.** 1 display font (menü, skor) + 1 body font (dialog metni). Palette 1 primary + 1 accent + 3 neutral. Ikonlar tek stil (rounded filled veya outlined — karışık değil).

**Somut örnek.** Royal Match: Lilita One benzeri painted display + sans-serif body. Palette: royal blue + gold accent. Icon set: rounded filled, custom painted.

**Anti-pattern.** Google Material ikonlarını doğrudan casual oyuna yapıştırmak → ucuz görünür. 5+ font ailesi → parse + boyut.

**Kural.** ≤2 font dosyası (bkz. `maui.md`, `performance.md`). Icon set tek, custom painted. Material/HIG referans alınır ama oyun için custom re-skin edilir.

## 12. FTUE & hook mechanisms
**Prensip.** Hook = ilk rewarding moment + "yarın ne olacak" teaser. Story, progress, collectibles üç ana hook.

**Somut örnek.** Royal Match: castle renovation teaser (level 2'de room unlock). Homescapes: Austin karakteri story cut. June's Journey: murder mystery. Hepsi oyuncuya bir "sonraki" söz verir.

**Anti-pattern.** Günlük reward popup 1. run'da. Hesap/e-posta isteme. İlk 2 run'da IAP CTA.

**Kural.** Hook D1'de kurulur, D2'de çağrılır. İlk gün reward popup'ı sadece level-end celebration.

## 13. Rewarded ad / IAP / energy / daily login — TIMING
**Prensip.** Opportunity-based, zorlama yok. Ad = kullanıcı izler, ödülü hak eder; popup = anlamlı moment.

**Somut örnekler.**
- **Rewarded ad** — level fail sonrası "continue with 5 moves", daily spin 2x, chest unlock skip. Her placement 30 s cooldown.
- **IAP popup** — ilk 3 run'da asla; 4. run'dan sonra starter pack 24 saat visible.
- **Energy** — hayat/enerji 5 kap, 20 dk regen. Tükenince popup: "Wait / Spend gems / Watch ad". "Watch ad" **ilk** seçenek olarak görsel ağırlıkta.
- **Daily login** — D2'den itibaren, sadece session başlangıcında 1 kez/gün. Skip X butonu sağ üst, 1 s delay.

**Anti-pattern.** Interstitial session start'ta (yasak, `monetization.md`). IAP popup gameplay ortasında. Daily reward tap-through zorunlu (skip yok). "Sadece bugün!" fake timer (dark pattern).

**Kural.** Tüm popup'lar queue'ya girer, session'da tek düzende: (1) daily login → (2) event teaser → (3) starter pack (eligible'ise). Bu sıra aşılmaz. Skip her zaman mümkün.

---

## Özet: 5 UX kuralı + 1 sistem önerisi

1. **60 saniye kuralı** — hesap yok, popup yok, reklam yok; ilk dakika oyuncuya bir başarı + bir teaser.
2. **Tek primary CTA** — her ekranda görsel ağırlıkta 1 buton; ikincil %60 opaklık.
3. **Popup queue** — oturum başına max 1 modal; sıra: daily → event → starter; skip her zaman mümkün.
4. **HUD ≤5 öğe** — currency + `+`, energy, level, rozet dot'ları, primary action; timer görselse progress bar.
5. **Accessibility default-on** — color + pattern, reduced motion toggle, text scale 100/115/130, system "Reduce Motion" takibi.

**Sistem önerisi — Paylaşılan `MGF.UI` kütüphanesi.** `tools/` altında bir MAUI class library: tek noktada `PrimaryButton`, `CurrencyPill`, `RewardModal`, `ToastService`, `PopupQueue`, `AccessibilityPrefs`, painted font + palette ResourceDictionary. Her oyun `games/<id>/src/<id>/` içinden `MGF.UI`'yi NuGet benzeri ProjectReference ile çeker; oyun-özel tema override eder. Bu yaklaşım (a) tutarlı UX, (b) yeniden yazma sıfır, (c) accessibility kuralları merkezi, (d) performans profili ortak (bkz. `performance.md`). MAUI Developer ilk oyun (`games/<id-1>/`) build kapısında bu kütüphaneyi bootstrap eder; sonraki oyunlar tüketir.

---

### Kaynaklar
- Deconstructor of Fun — Royal Match teardown: https://www.deconstructoroffun.com/blog/2021/3/21/royal-match-the-new-king-from-turkey
- UserWise — 5 UX Lessons from Royal Match: https://blog.userwise.io/blog/5-simple-ux-lessons-from-royal-match
- Sensor Tower — Royal Match vs Candy Crush: https://sensortower.com/blog/royal-match-surpasses-candy-crush-saga-in-revenue-and-downloads-for-the
- GameAnalytics — FTUE tips for F2P: https://www.gameanalytics.com/blog/tips-for-a-great-first-time-user-experience-ftue-in-f2p-games
- Udonis — FTUE + Rewarded ad placements: https://www.blog.udonis.co/mobile-marketing/mobile-games/first-time-user-experience
- Udonis — 16 Rewarded Video Placements: https://www.blog.udonis.co/mobile-marketing/mobile-games/rewarded-video-ad-placements
- Game UI Database: https://gameuidatabase.com/
- Accessible Game Design — HUD guidelines: https://accessiblegamedesign.com/guidelines/HUD.html
- Access-Ability 2025 recap: https://access-ability.uk/2025/12/05/2025-video-game-accessibility-recap/
- Material Design 3 — Dialogs: https://m3.material.io/components/dialogs/guidelines
- Material Design 3 — Navigation bar: https://m3.material.io/components/navigation-bar/guidelines
- Apple HIG — Tab bars: https://developer.apple.com/design/human-interface-guidelines/tab-bars
- LogRocket — Modal UX patterns: https://blog.logrocket.com/ux-design/modal-ux-design-patterns-examples-best-practices/
- Mobbin — Empty state UI: https://mobbin.com/glossary/empty-state
- DesignTheGame — Hyper-casual vs mid-core: https://www.designthegame.com/learning/tutorial/the-hyper-casual-phenomenon-differentiating-traditional-mobile-games-developers
- Medium — Juicy UI (Mezo Istvan): https://medium.com/@mezoistvan/juicy-ui-why-the-smallest-interactions-make-the-biggest-difference-5cb5a5ffc752
- dev.to — Micro-interaction rules 2026: https://dev.to/devin-rosario/5-micro-interaction-design-rules-for-apps-in-2026-48nb
- AppSamurai — Ad placement best practices 2025: https://appsamurai.com/blog/best-practices-of-video-ad-placement-in-mobile-games/
- Tenjin — Rewarded Ads 101: https://tenjin.com/blog/rewarded-ads-101-launch-and-scale-with-proven-best-practices/
- DesignShack — Dark mode typography: https://designshack.net/articles/typography/dark-mode-typography/
- Medium — Design Deep Dive Royal Match (Saravanan): https://medium.com/ironsource-levelup/design-deep-dive-02-royal-match-948f7af96f04
