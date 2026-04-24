# LiveOps Calendar — {{title}}

**ID**: `{{id}}`
**LiveOps Manager**: {{name}}
**Tarih**: {{YYYY-MM-DD}}
**Kapı**: liveops

---

## 1. Daily rotation (7 gün)

| Gün | Quest tipi | Ödül |
|---|---|---|
| Pazartesi | Match quest (N match) | 50 coin |
| Salı | Combo quest (chain ≥3) | 75 coin + 1 hint |
| Çarşamba | Time attack | 100 coin |
| Perşembe | Puzzle variant | 1 boost |
| Cuma | Event preview | 2x XP |
| Cumartesi | Weekend bonus (2x reward) | 2x coin all day |
| Pazar | Weekend bonus devam | 2x coin all day |

Streak: 3 gün (50 coin), 7 gün (200 coin), 14 gün (rare cosmetic).

## 2. Weekly event queue (12 hafta önden planlı)

### Hafta 1 — Launch Tournament
- 7 gün leaderboard
- Top 100 → cosmetic skin
- Top 10 → rare skin + 500 coin
- Top 1 → legendary title

### Hafta 2 — Hızlı Keşif
- 72 saat mini event
- Objective: 50 stage/gün tamamla
- Reward: 1 epic reward chest

### Hafta 3 — Volkanik Görev
- 7 gün tournament
- Event stage variant (volkan)
- ...

### ...devam 12 hafta

## 3. Weekend IAP bundle
Her Cuma 18:00 - Pazar 23:59:
- %20-40 discount 1 SKU
- Rotate: energy pack → starter → remove ads → coin mega

## 4. Mini battle pass (4 hafta cycle)

### Free track (30 tier)
- Tier 1-10: coin, hint
- Tier 11-20: boost, chest key
- Tier 21-30: character skin, event-exclusive

### Premium track ($4.99)
- Instant Tier 1 reward
- Her tier +1 bonus
- Tier 30: epic skin

### XP
- Stage complete: +10
- Daily quest: +20-50
- Event quest: +100
- Tournament top 10: +200

Casual oyuncu ≈107 XP/gün → 30 tier / 28 gün ulaşılır.

## 5. Seasonal palette (aylık)
- Ocak-Mart: Winter (cool tones)
- Nisan-Haziran: Spring (pastel)
- Temmuz-Eylül: Summer (vibrant)
- Ekim-Aralık: Fall (warm)

## 6. Push notification template

| Segment | Timing | Mesaj |
|---|---|---|
| Non-payer 24h inactive | 19:00 local | "Enerjin doldu! Macera bekliyor 🗺️" |
| Payer 48h inactive | 19:30 local | "Yeni event başladı — 72 saat!" |
| Churned 7d+ | 20:00 local | "Seni özledik! Yeni bölge açıldı 🌴" |
| Event start | 10:00 local | "{EventName} başladı!" |
| Streak | 09:00 local | "Streak devam! Bugün {N}. gün ⭐" |
| Daily reset | 08:00 local | "Yeni günün hediyesi açıldı 🎁" |

Max 2 push/gün, 9-21 local time.

## 7. LTO (Limited Time Offer)
- Starter Pack: launch + 24h (2 pencere)
- Weekend Bundle: Cuma 18:00 - Pazar 23:59
- Post-loss: 3 fail arka arkaya → 1 ucuz boost (75% discount)
- Whale: LTV >$100 monthly bundle

## 8. KPI monitoring (haftalık)
| Metric | Target | Alarm |
|---|---|---|
| D1 retention | ≥40% | <30% = P0 |
| D7 retention | ≥15% | <10% = P0 |
| ARPDAU | ≥$0.15 | <$0.08 = P1 |
| Payer conversion | ≥2.5% | <1% = P1 |
| D7 ROAS | ≥8% | <4% = P0 |
| Crash-free | ≥99.5% | <99% = P0 |
| Session length avg | ≥3 min | <2 min = P1 |
| Sessions/DAU | ≥3 | <2 = P1 |

Alarm <target: incele + rollback log.

## 9. Ops yükü
Hedef: ≤8 saat/hafta
- Günlük: 0 dk (otomatik sayaç)
- Haftalık: 2 saat (event config + push)
- Aylık: 4 saat (battle pass + palette)
