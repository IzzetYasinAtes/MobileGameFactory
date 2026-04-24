# Live Ops, Growth & Monetization — Research (2025-2026)

**Kapsam**: Mobil oyun live ops anatomisi, monetization, UA, ASO, analytics, A/B, push, dark pattern yasakları ve 2025-2026 sektör trendleri. Tek kişilik studio MobileGameFactory için normatif bir referans çıkarmak amacıyla Sensor Tower, GameAnalytics, Adjust, Business of Apps, Naavik, Tap Nation, Airbridge ve Segwise kaynakları sentezlendi.

---

## 1. Live Ops Anatomisi

Live ops, ship'ten sonra oyunu canlı tutan **zaman çizelgesi**dir. 2025 Sensor Tower "Winning with Live Ops" raporu üç katman tanımlar:

1. **Recurring events (günlük iskelet)**: daily check-in, daily quests (3 görev, ~5 dk), streak sayacı, win-streak bonusu, limited-time offer (LTO) pop-up'ı.
2. **Feature events (haftalık ritim)**: mini-game, hunt, bingo, gacha, tournament, collab (IP cross-over). Battle pass bu katmanın omurgası.
3. **Tent-pole events (aylık/sezonluk zirve)**: sezon finali, server-wide milestone, PvP championship, mega collection. Premium monetization tetikleyici.

**Battle pass** modern standart. Unity/Fortnite verisi: minimum **30 gün**, ücretsiz + premium iki hat (ikincisi 4.99-9.99 USD), 12-18% aktif kullanıcıyı dönüştürüyor. Fortnite battle pass'i D90 retention'ı %78'e çıkardı; bu mobildeki başarı örneği. İyi battle pass tek başına gelir değil, **kalış sebebi** üretir.

Küçük studio realitesi: daily quest + weekly event + 4-haftalık mini battle pass üçlüsü, hypercasual+ için yeter. Collab ve tournament ileri kapıdır; LTV kanıtlanmadan planlanmaz.

## 2. Monetization 2025-2026

**Hibrit casual** 2025'in net galibi: IAP revenue +37% YoY, ARPDAU 0.50-1.00+ USD. Hypercasual revenue +80% (hibritleşme sayesinde) ama pure-hypercasual ARPU 0.86 USD'de sıkıştı (Tenjin 2025). Merge-3 gibi hibrit alt türler ad-ARPU 14.83 USD'ye ulaşıyor.

**Reklam karışımı**:
- **Rewarded video**: ad gelirinin %62'si, engagement 45-60%. Her zaman opt-in, değer teklifi net. ← Ağırlık merkezi.
- **Interstitial**: 4 dk aktif oyun / 1 görüntüleme; ilk 3 run'da yok; session start'ta yok.
- **Banner**: küçük oyun için varsayılan KAPALI; sadece menu/shop, gameplay'de yasak.
- **App-open**: retention düşürüyor, varsayılan KAPALI.
- **Offerwall**: mid-core için ek gelir; casual'de gereksiz karmaşa.

**IAP mimarisi**: 0.99 / 2.99 / 4.99 / 9.99 tier, en üst tavan 19.99. Remove-ads (2.99), cosmetic, permanent QoL, soft-currency paketi, 24-saat starter pack. Subscription/VIP kulüpleri 2026'da mainstream oldu ama küçük session oyun için overkill.

**Hibrit altın oran**: casual ~50/50 IAP:Ad, hypercasual ~90/10 Ad:IAP, hybrid-casual 40-50% IAP. Proje ölçeğimiz için **hedef IAP:Ad ≈ 30:70** başlangıç; 60. günde ölç ve kaydır.

## 3. Metrik Tablosu (2026 benchmark — iOS + Android karışık)

| Metrik | Zayıf | Ortalama | İyi | Üst %10 |
|---|---|---|---|---|
| D1 retention | <30% | 35-40% | 45-50% | 55%+ |
| D7 retention | <10% | 12-15% | 18-22% | 25%+ |
| D30 retention | <3% | 5-7% | 8-10% | 12%+ |
| ARPDAU (casual) | <$0.05 | $0.08-0.15 | $0.20-0.30 | $0.50+ |
| ARPDAU (hybrid) | <$0.30 | $0.50-0.80 | $1.00 | $1.50+ |
| ARPPU | <$5 | $10-15 | $20-30 | $50+ |
| Payer conversion | <1.5% | 2-3% | 4-5% | 7%+ |
| Whale ratio (%pay giving %rev) | — | top 10% → 60% | top 5% → 70% | top 2% → 80% |
| CPI Android (casual) | >$2 | $0.80-1.50 | $0.40-0.80 | <$0.30 |
| CPI iOS (casual) | >$6 | $3-5 | $2-3 | <$2 |
| D7 ROAS | <5% | 7-9% | 12-15% | 20%+ |
| Payback period | >180 gün | 120-150 | 60-90 | <45 |

Kaynaklar: GameAnalytics 2025 Benchmarks, Tenjin, Mapendo, Business of Apps, Game Growth Advisor.

## 4. User Acquisition

**Kanal mikseri**: AppLovin (video+playable), Unity Ads, ironSource, TikTok (Gen Z + UGC formatı), Meta (geniş açı), Mintegral (APAC), Google Ads (ASO+UAC). Küçük studio için **AppLovin + TikTok + Meta** üçlüsü standart; günlük bütçe 20-50 USD/test.

**Creative testing**: ad group başına 10-15 creative, haftada 15-25 yeni variant, **multivariate** (MVT) artık A/B'nin yerine geçti. 2026 gerçeği: creative'lerin %50+'sında AI hook var, tamamen AI-generated oran yükselişte (Segwise). Playable ad hybrid casual'de retention'ı UA tarafında 1.3-1.8x çarpıyor.

**IPM** (installs per mille) TikTok/AppLovin için sağlıklı aralık 15-30; altına düştüyse creative ölü.

## 5. ASO

**Anahtar kelime**: Apptweak/AsoDesk/Sensor Tower ile 4-6 haftada bir refresh. Title'da volume'lu 1 primary + subtitle'da 2 long-tail.

**Icon**: A/B test store'da (Play Console Experiments). Basit, rakiplerden ayrışık. Renk: mavi %23 güven, mor = distinctiveness.

**Screenshot**: ilk 3 tane zero-english, hook'lu; video preview 15-30 sn silent-first (ses izni yok).

**Lokalizasyon önceliği**: EN, TR, DE, FR, ES, PT-BR, JA, KO, zh-CN, AR (RTL). Sadece string değil, **screenshot'ları da lokalize et** — conversion +5-15%, tam lokalize organik install +10-25%.

## 6. Analytics Stack

Minimum üçlü:
1. **Attribution**: AppsFlyer veya Adjust (SKAdNetwork + postback, UA kanallarının ROAS'ı için tek gerçek kaynak).
2. **Product analytics**: GameAnalytics (bedava, gaming-native, 14.000+ titleda benchmark) → başlangıç için ideal. İleride Amplitude/Mixpanel.
3. **Crash + Remote Config + A/B**: Firebase (Crashlytics, Remote Config, A/B Testing — hepsi bedava). Pomelo Games Remote Config ile interstitial A/B'sini ikiserde koştu, reklam geliri +25%, IAP +35%.

Kurum olarak **backend yok** politikamızla çelişmeyen tek kombinasyon budur: AppsFlyer/Adjust SDK + GameAnalytics SDK + Firebase SDK = 3 SDK, hepsi client-side event, local-first kuralı korunur.

## 7. Push Notifications

**75.8%** takım, push'un ilk 30 gün retention'a en büyük katkıyı yaptığını söylüyor (2026 State of Customer Engagement). Behavior-triggered push, standart gönderimi **9x** CTR'da geride bırakıyor.

Segment şablonu: `active_today`, `lapsed_3d`, `lapsed_7d`, `lapsed_30d`, `paid_whale`, `non_payer_high_engagement`, `streak_at_risk`. Timing: kullanıcı timezone + geçmiş aktif saat. Gün başı 1, akşam 1 tavan. Streak-at-risk en güçlü hook.

iOS opt-in %60, Android %95 ama iOS open-rate daha yüksek — iOS push prompt'u **ilk session'da asla isteme**; 2-3 anlamlı sessiondan sonra sor.

## 8. A/B Testing

İki alan:
- **Live ops A/B**: offer fiyatı, timing, battle pass reward curve, difficulty ramp. Firebase Remote Config + A/B Testing default stack. 14 gün, min 1000 user/variant, tek metric north-star.
- **UA creative A/B**: Meta/TikTok built-in; MVT için Segwise / Appsumo benzeri creative intelligence. 7 gün, IPM + D1 retention hedef.

**Kural**: aynı anda max 2 deney/oyun. Sonuç karışır, istatistik ölür.

## 9. Retention Mekanikleri

- **Daily streak** + comeback bonus (3 gün yok → "kaçırdığın rewardlar" paketi).
- **Daily check-in ladder** (7 gün loop, 7. gün büyük).
- **Friends/teams/alliance** — küçük oyunda hafif versiyon (Game Center/Play Games leaderboard + tek buton challenge).
- **Progression meta** (level, skin, house) — core loop üstünde meta katman.
- **Live event countdown** (gerçek, sahte değil).

## 10. Yasak Dark Pattern Listesi

FTC 2026 Negative Option Rule ve Epic Games 245M USD cezası ışığında:

- Sahte countdown timer (gerçekten bitmeyen "24 saat!").
- Bait-and-switch (gösterilen reward ≠ alınan).
- Confusing currency (gem → coin → token → ticket çoklu para, kasıtlı karmaşa).
- Ters renkli/konumlu onay butonu (iptal gri, satın al yeşil abartı).
- "Gizli" iptal akışı (subscription için support-ticket şartı).
- Ayarlarda reklamı açık varsayılan "personalized ads" (GDPR ihlali).
- Çocuk hedefli oyunda IAP (COPPA).

Bu liste `.claude/rules/monetization.md` ile bağlanır; monetization-audit skill'i ship öncesi her oyunu tarar.

## 11. 2025-2026 Sektör Trendi

- **Hibrit casual = yeni ortalama**. Pure hypercasual ölmedi ama economic moat yok.
- **AI UA creatives**: 2026 sonu %50+ creative AI-assisted; küçük studio için eşitleyici.
- **UGC**: hafif UGC (paylaşılabilir skor, replay clip) mainstream; tam UGC platformu (Roblox-benzeri) küçük studio için ulaşılmaz.
- **Web3**: sessiz dönüş, interoperable asset vaadi; küçük casual oyun için **uygulamaz** (regulator riski + user trust ↓). Red-list.
- **Subscription/VIP**: büyüklerde standart; bizim ölçekte ship sonrası ölç, gerek görürsen ekle.

---

## Event Cadence Takvim Örneği (4 haftalık mini season)

| Gün | Event | Tipi | Hedef |
|---|---|---|---|
| Her gün | Daily check-in + 3 daily quest | Recurring | D1 retention, streak |
| Pzt | Weekly tournament başlar (7 gün) | Feature | D7 retention, competitive |
| Çar | Rewarded ad 2x coin (12 saat) | Recurring | Ad impression tavanı |
| Cum | Mini-event (bingo/hunt 72 saat) | Feature | Session frequency |
| Paz | Weekend IAP bundle (-30%) | Recurring | ARPPU |
| 7/14/21 | Battle pass tier-up reward push | Feature | Re-engagement |
| 28 | Season finale + leaderboard reward + teaser next season | Tent-pole | Retention bridge |

Bu cadence ayda ~10 event, tek kişi ops yükü ≤8 saat/hafta (content önceden pişirilmiş ise).

---

## Kaynaklar
- Sensor Tower — Winning with Live Ops 2025
- GameAnalytics — 2025/2026 Mobile Gaming Benchmarks
- Adjust — Live Ops Strategy Guide
- Naavik — Mobile Puzzle Live Ops Trends, State of UGC 2026
- Tenjin — Ad Mon Gaming Benchmark 2025
- Tap Nation, Airbridge — Hybrid casual KPIs
- Business of Apps — Retention Rates 2026, Push Services 2026
- Mapendo, Segwise — CPI / IPM / ROAS benchmarks
- Udonis, Appalize, Apptweak — ASO 2026 guides
- Firebase Blog — Remote Config + A/B case studies (Pomelo, Halfbrick)
- FTC — Dark Patterns Report, Negative Option Rule 2026
