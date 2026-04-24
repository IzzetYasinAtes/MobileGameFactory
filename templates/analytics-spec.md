# Analytics Spec — {{title}}

**ID**: `{{id}}`
**Data Analyst**: {{name}}
**Tarih**: {{YYYY-MM-DD}}
**Kapı**: analytics

---

## 1. Event taxonomy

### Session
- `session_start`: session_id, device, os_version, app_version, locale
- `session_end`: duration_sec, reason

### Gameplay
- `stage_start`: stage_id, attempt_number, world
- `stage_complete`: stage_id, moves_used, time_sec, stars, score
- `stage_fail`: stage_id, reason (moves_out / time_out / giveup)
- `stage_retry`: stage_id, attempt_number
- `stage_bailout`: stage_id, moves_remaining, progress_pct
- `hint_used`: stage_id, hint_type
- `boost_used`: stage_id, boost_type

### Progression
- `level_up`: new_level, xp_earned
- `world_unlock`: world_id, trigger
- `character_unlock`: character_id, cost_type
- `achievement_unlock`: achievement_id

### Merge
- `merge_success`: chain, tier, combo_length
- `combo_chain_n`: n

### Economy
- `currency_earn`: currency_type, amount, source
- `currency_spend`: currency_type, amount, item_bought
- `item_acquire`: item_id, source
- `item_use`: item_id, context

### Ads
- `ad_request`: placement, network
- `ad_impression`: placement, network, revenue_usd
- `ad_click`: placement, network
- `ad_reward_granted`: placement, reward_type, reward_amount
- `ad_error`: placement, error_code

### IAP
- `iap_initiated`: sku, price_tier
- `iap_completed`: sku, price_usd, transaction_id (hashed)
- `iap_failed`: sku, error
- `iap_restored`: sku

### UI
- `screen_view`: screen_name, duration_sec
- `button_tap`: button_id, screen (key buttons)
- `popup_open`: popup_type, priority
- `popup_dismiss`: popup_type, reason

### Engagement
- `daily_login`: streak_count
- `streak_continue`: streak_count
- `notification_open`: notification_type, campaign_id

### LiveOps
- `event_start`: event_id, event_type
- `event_complete`: event_id, score, rewards
- `battle_pass_tier_claim`: tier, track
- `battle_pass_buy`: pass_id

## 2. Funnel tanımları

### Install → D1 Return
```
install → tutorial_start → tutorial_complete → session_2_start → D1_return
```

### D1 → D7
```
D1_return → daily_login_3x → stage_complete_{N} → D7_return
```

### F2P → Payer
```
session_5+ → IAP_popup_seen → IAP_initiated → IAP_completed
```

### Rewarded → Retention
```
ad_reward_granted → next_session_start (24h) → session_length_delta
```

## 3. Retention cohort
- D1 (install +1 session 24-48h)
- D7 (install +7 session)
- D14 (install +14 session)
- D30 (install +30 session)
- Payer (≥1 IAP)
- Whale (≥$50 cumulative LTV)
- Retained payer (payer + D7+)

## 4. LTV model

```
LTV(D90) =
  D1_R × ARPDAU_D1 × 1 +
  D7_R × ARPDAU_D7 × 6 +
  D14_R × ARPDAU_D14 × 7 +
  D30_R × ARPDAU_D30 × 16 +
  D90_R × ARPDAU_D90 × 60
```

Payback: 90 günde LTV ≥ CPI × 1.5.

## 5. A/B test queue
1. Onboarding tutorial length (A=short B=long)
2. Starter pack price ($2.99 vs $4.99)
3. Difficulty curve W1 S5-10 (easier vs harder)
4. Event timing (weekly vs 72h)
5. Reward chest type (soft vs hard vs character skin)

## 6. Stack

### Primary
- **Firebase Analytics** (ücretsiz, unlimited events, BigQuery export)
- **Firebase Crashlytics** (opsiyonel)

### Secondary
- **GameAnalytics** (casual dashboards)

### UA attribution (opsiyonel)
- Appsflyer veya Adjust

### Product analytics
- Amplitude (free tier 10M events/month)

## 7. Privacy / consent

### GDPR
- Consent banner ilk açılış
- "Kabul et" / "Sadece gerekli"
- Data delete request (Settings)

### CCPA
- "Do Not Sell My Data" opt-out

### ATT (iOS 14.5+)
- IDFA dialog

### COPPA
- `setTagForUnderAgeOfConsent(true)`
- No behavioral ad targeting

### Veri saklama
- Local: event queue 7 gün
- Cloud: 14 ay default
- PII: asla cloud'a, hash zorunlu

## 8. Dashboard setup
- Firebase Studio: install/DAU/retention chart
- Amplitude: funnel + cohort
- GameAnalytics: stage heatmap
