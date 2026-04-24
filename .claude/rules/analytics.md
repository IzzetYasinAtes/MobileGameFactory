# Analytics Kuralları

## Sert kurallar
1. **PII log YASAK** (email, IP, konum adı) — hash/anonymize zorunlu
2. **Event count ≤10/session** (cost + perf)
3. **Consent flow zorunlu** (GDPR, CCPA, ATT, COPPA)
4. **Local-only prensip korunur** — backend yok, analytics SDK'lar opsiyonel + opt-out
5. **Vendor lock-in yok** — event payload platform-agnostik

## Event taxonomy (standart)

### Session
- `session_start` → params: `session_id`, `device`, `os_version`, `app_version`, `locale`
- `session_end` → params: `duration_sec`, `reason` (backgrounded / closed)

### Gameplay
- `stage_start` → `stage_id`, `attempt_number`, `world`
- `stage_complete` → `stage_id`, `moves_used`, `time_sec`, `stars`, `score`
- `stage_fail` → `stage_id`, `reason` (moves_out / time_out / giveup)
- `stage_retry` → `stage_id`, `attempt_number`
- `stage_bailout` → `stage_id`, `moves_remaining`, `progress_pct`
- `hint_used` → `stage_id`, `hint_type`
- `boost_used` → `stage_id`, `boost_type`

### Progression
- `level_up` → `new_level`, `xp_earned`
- `world_unlock` → `world_id`, `trigger` (auto/manual)
- `character_unlock` → `character_id`, `cost_type` (ad / iap / progression)
- `achievement_unlock` → `achievement_id`

### Merge / Core mechanic
- `merge_success` → `chain`, `tier`, `combo_length`
- `combo_chain_n` → `n` (combo size)

### Economy
- `currency_earn` → `currency_type`, `amount`, `source`
- `currency_spend` → `currency_type`, `amount`, `item_bought`
- `item_acquire` → `item_id`, `source`
- `item_use` → `item_id`, `context`

### Ads
- `ad_request` → `placement`, `network`
- `ad_impression` → `placement`, `network`, `revenue_usd` (if LTV tracking)
- `ad_click` → `placement`, `network`
- `ad_reward_granted` → `placement`, `reward_type`, `reward_amount`
- `ad_error` → `placement`, `error_code`

### IAP
- `iap_initiated` → `sku`, `price_tier`
- `iap_completed` → `sku`, `price_usd`, `transaction_id` (hashed)
- `iap_failed` → `sku`, `error`
- `iap_restored` → `sku`

### UI
- `screen_view` → `screen_name`, `duration_sec`
- `button_tap` → `button_id`, `screen` (ana button'lar, tüm tıkları değil)
- `popup_open` → `popup_type`, `priority`
- `popup_dismiss` → `popup_type`, `reason` (skip/close/action)

### Engagement
- `daily_login` → `streak_count`
- `streak_continue` → `streak_count`
- `notification_open` → `notification_type`, `campaign_id`

### LiveOps
- `event_start` → `event_id`, `event_type`
- `event_complete` → `event_id`, `score`, `rewards`
- `battle_pass_tier_claim` → `tier`, `track` (free/premium)
- `battle_pass_buy` → `pass_id`

## Funnel tanımları

### Install → D1 Return
```
install → tutorial_start → tutorial_complete → session_2_start → D1_return (24-48h later)
```

### D1 → D7
```
D1_return → daily_login_3x → stage_complete_{N} → D7_return (168h later)
```

### F2P → Payer
```
session_5+ → IAP_popup_seen → IAP_initiated → IAP_completed (first purchase)
```

### Rewarded → Retention
```
ad_reward_granted → next_session_start (within 24h) → session_length_delta
```

## Retention cohort tanımları
- **D1**: install day +1 session (24-48h window)
- **D7**: install day +7 session
- **D14**: install day +14 session
- **D30**: install day +30 session
- **Payer**: ≥1 IAP yapmış (cohort'tan bağımsız)
- **Whale**: ≥$50 cumulative LTV
- **Retained payer**: payer + D7+ active

## LTV model (basit)

```
LTV(D90) = 
  D1_R × ARPDAU_D1 × 1 +
  D7_R × ARPDAU_D7 × 6 +
  D14_R × ARPDAU_D14 × 7 +
  D30_R × ARPDAU_D30 × 16 +
  D90_R × ARPDAU_D90 × 60
```

**Payback hedefi**: 90 günde LTV ≥ CPI × 1.5 (kar marjı %50+)

## A/B test framework

### Kurallar
- **Max 3 concurrent test** (cannibalization önle)
- **Split**: 50/50 control vs variant
- **Sample size**: min 500/group, statistical significance 95%
- **Duration**: 7–14 gün
- **One variable per test** (çoklu değişken = noise)

### Testing queue örneği
1. Onboarding variant A vs B (tutorial length)
2. Starter pack price ($2.99 vs $4.99)
3. Difficulty curve stage 5-10 (easier vs harder)
4. Event timing (haftalık vs 72 saatlik)
5. Reward chest type (soft currency vs hard currency vs character skin)

### Winner selection
- Primary metric: **D7 retention** (en kritik)
- Secondary: ARPDAU, payer conversion
- Significance gate: 95% confidence interval
- Kazanan scale: 100% traffic'e; kaybeden log + kill

## Analytics stack

### Primary: Firebase Analytics
- Ücretsiz, unlimited events
- BigQuery export (SQL analysis)
- Crashlytics integration
- A/B testing native (Remote Config)

### Secondary: GameAnalytics
- Ücretsiz tier (100K DAU'ya kadar)
- Casual-optimized dashboards
- Heatmap (stage completion)
- Funnel wizard

### UA Attribution
- **Appsflyer** — industry standard, paid
- **Adjust** — alternatif, paid
- **Firebase dynamic links** — ucuz, sınırlı

### Product analytics
- **Amplitude** — free tier (10M events/month), güçlü cohort
- **Mixpanel** — free tier (20M events/month), funnel strong

### Crash / performance
- **Firebase Crashlytics** — ücretsiz, iOS + Android
- **Sentry** — alternatif, hybrid

## Privacy / consent

### GDPR (EU)
- Consent banner ilk açılışta
- 2 seçenek: "Kabul et" / "Sadece gerekli"
- Reddedince: sadece essential (session, crash); analytics opt-out
- Data delete request: Settings → "Verilerimi Sil"

### CCPA (California)
- "Do Not Sell My Data" opsiyonu
- Opt-out link (Settings)

### ATT (iOS 14.5+)
- IDFA permission dialog
- Reddedince: non-personalized ads, analytics anonymize

### COPPA (under 13)
- Child-directed flag (`setTagForUnderAgeOfConsent(true)`)
- No behavioral ad targeting
- No social features

### Veri saklama
- **Local**: event queue 7 gün (offline fallback), sonra sil
- **Cloud (Firebase/Amplitude)**: 14 ay default, 24+ ay isteğe
- **PII**: asla cloud'a gitmez — hash zorunlu

## Yasaklar
- PII log plaintext (email, IP, name)
- Over-track (>100 event type)
- Vendor lock-in (tek SDK)
- Consent-bypass
- Event spam (>10/session)
- Backend-exposed key (API key hardcoded)
