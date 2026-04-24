# Live Ops Kuralları

## Sert kurallar
1. **Her ship edilmiş oyun min**: daily check-in + weekly event + 4 haftalık battle pass
2. **Banner + App-open reklam default KAPALI**
3. **Rewarded opt-in zorunlu** (kullanıcı butonu basar)
4. **Interstitial**: 1/4 dk active game time, ilk 3 run yok, core loop içinde yok
5. **Ops yükü ≤8 saat/hafta** (content pre-baked ship'te)
6. **FTC dark pattern blacklist uygulanır** — monetization-audit skill'i otomatik kontrol eder

## Minimum cadence (tek PM, küçük studio)

### Günlük
- **Daily check-in**: 5/6/7 gün streak bonus, reset UTC 00:00 (veya local 04:00)
- **3 daily quest**: level-specific veya skill-specific (örn: "5 merge yap", "2 stage bitir", "1 boost kullan")
- **Streak bonus**: 3 gün (küçük), 7 gün (orta), 14 gün (büyük ödül — rare cosmetic)

### Haftalık
- **1 weekly tournament** (7 gün leaderboard, top 100 rewards)
- **1 mini event** (72 saat limited, unique objective + reward)
- **Weekend IAP bundle** (Cuma-Pazar %20–40 discount 1 SKU)

### 4 haftalık
- **Mini battle pass**: 30 tier free + 30 tier premium ($4.99)
- **Season finale event**: son hafta büyük kutlama, özel ödül
- **Seasonal palette shift** (aylık): spring/summer/fall/winter

### Ops yükü hedefi
- **Günlük**: 0 dakika (otomatik sayaçlar)
- **Haftalık**: 2 saat (event config + push schedule)
- **Aylık**: 4 saat (battle pass + palette)
- **Toplam**: ≤8 saat/hafta

## Event content pipeline

### Pre-baked content (ship'te gömülü)
Event content'i **ship AAB'sinde gömülü** olmalı — canlıya alım için update gerekmez. Config'le aktive edilir (`events.json`):
- Event banner asset
- Event icon + sprite variant
- Music loop
- Reward definitions
- Event-specific stage varyantı (eğer stage-based event ise)

### Server config (opsiyonel, local-only prensipte offline fallback)
- `events.json` app içinde default; uzak config (Firebase Remote Config veya local CDN) override edebilir
- Offline fallback: son çekilen config cache'lenir, internet olmadığında çalışır

## Push notification stratejisi

### Kurallar
- Max **2 push/gün** (exceed → user opts out)
- Local time 9:00–21:00 (diger saatler spam)
- Segment: non-payer 24h inactive, payer 48h inactive, churned 7d+
- Message personalize (name, progress, reward)
- **Opt-out 1 tap** (Settings → Bildirimler)

### Template örnekler
- **Energy full**: "Enerjin doldu! Macera bekliyor 🗺️"
- **Daily reset**: "Yeni günün hediyesi açıldı 🎁"
- **Event start**: "Volkanik Event başladı — 72 saat!"
- **Streak**: "Streak devam! Bugün 5. günün ⭐"
- **Re-engage (D3 no-return)**: "Seni özledik! Yeni bölge açıldı 🌴"

## Battle Pass yapısı (30 tier × 2 track)

### Free track
- Tier 1–10: küçük ödüller (coin, hint)
- Tier 11–20: orta ödüller (boost, chest key)
- Tier 21–30: büyük ödüller (character skin, event-exclusive)

### Premium track ($4.99)
- Instant unlock Tier 1 özel reward
- Her tier'da +1 bonus reward
- Tier 30 özel reward (epic skin veya rare item)
- Tier 31–40 unlocked ("premium+" bonus)

### XP kazanımı
- Her stage complete: +10 XP
- Daily quest: +20–50 XP
- Event quest: +100 XP
- Weekly tournament top 10: +200 XP

Tier başına ortalama 100 XP → 3000 XP / 30 tier / 28 gün = 107 XP/gün. Casual oyuncu daily quest + 2 stage ile ulaşır.

## LTO (Limited Time Offer) stratejisi
- **Starter Pack**: launch + 24h + oyun başından 24h sonra (2 pencere)
- **Weekend Bundle**: Cuma 18:00–Pazar 23:59 (1 SKU, %20–40 discount)
- **Post-loss offer**: stage fail 3 kez arka arkaya → "1 kez ucuz boost paketi" (75% discount, 1 kez/hafta)
- **Whale offer**: LTV >$100 oyuncular için custom premium bundle (aylık 1 kez)

**Yasak LTO pattern**:
- Fake "son 5 dakika!" timer (gerçek deadline olmalı)
- "Sadece sana!" bait-and-switch
- Rotating "new" offer (aynı SKU yeni paketmiş gibi)

## KPI monitoring (haftalık rapor)

| Metric | Target | Alarm |
|---|---|---|
| D1 retention | ≥40% | <30% = P0 |
| D7 retention | ≥15% | <10% = P0 |
| ARPDAU | ≥$0.15 | <$0.08 = P1 |
| Payer conversion | ≥2.5% | <1% = P1 |
| D7 ROAS | ≥8% | <4% = P0 (UA kes) |
| Crash-free rate | ≥99.5% | <99% = P0 |
| Session length avg | ≥3 min | <2 min = P1 |
| Sessions/DAU | ≥3 | <2 = P1 |

Metrik **<target**: LiveOps Manager incele, `log_append(decision="rollback")` ve önceki config'e dön.

## FTC / yasak patternler
- Sahte timer ("Bu teklif sadece 10 dakika!" ama aslında sürekli)
- Bait-and-switch offer (görünen fiyat != ödenen)
- Confusing currency (çok currency tipi, oyuncu karışık)
- Rotating currency (günlük reset, oyuncu biriktirmesin)
- Paywall tutorial (ilk oyun IAP zorunlu)
- Hidden auto-renewal (subscription tek tap)
- Child-directed pay-to-win (COPPA ihlali)
- Grind wall (aynı stage N kez tekrar gerektirir)

İhlal tespit edilirse → ship blocker, content geri çek.

## Yasaklar (kesin)
- Push spam (>2/gün)
- Event content post-ship eklemek (AAB update push)
- Pay-to-progress event (sadece payer ilerler)
- Weekend reklam-free/event-free (weekend = peak engagement)
- Dark pattern listesi (yukarı)
