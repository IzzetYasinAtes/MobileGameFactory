# Stage Plan — {{title}}

**ID**: `{{id}}`
**Durum**: design / build / qa / shipped
**Target genre**: {{match-3 / merge / hyper-casual / idle / puzzle / adventure}}
**Launch hedefi**: {{stage_count}} stage, {{world_count}} world/biome

---

## 1. World (Biome) listesi
Her world tek satır özet. Tür bazında min/max:
- Match-3 / Merge live-ops: **3–5 world**, her world 20–40 stage (launch ~100–120)
- Hyper-casual: **1 world**, 30–50 stage
- Idle: **10–15 world** + prestige layer
- Premium puzzle: **3–5 world**, her world 5–7 stage (launch 15–25)

| # | World adı | Tema | Mekanik twist | Görsel palet (3 hex) | Stage sayısı |
|---|---|---|---|---|---|
| 1 | Tropik Orman | Başlangıç, ipucu bol | Temel merge | #1F7A6C/#F5A623/#C6E0C0 | 20 |
| 2 | ... | | | | |

---

## 2. Difficulty curve (world bazında)
Örnek: World 1
- Stage 1–5: **tutorial** — adım adım yeni mekanik tanıtımı, target first-try **%85+**
- Stage 6–12: **normal** — 2 mekanik birleşim, target first-try **%55–75**
- Stage 13–17: **hard** — 3+ constraint, target first-try **%30–45**
- Stage 18–19: **super-hard** — limited moves / time, target first-try **%20–30**
- Stage 20: **boss** — unique obstacle + reward, target first-try **%15–25**

---

## 3. Stage listesi (her stage için tam kayıt)

### Stage 1 — Ilk Tohum
| Alan | Değer |
|---|---|
| `stage_id` | 1-01 |
| `world` | 1 (Tropik Orman) |
| `tier` | tutorial |
| `variant` | 2-match-teach |
| `objective` | 2x Stone T2 üret |
| `new_mechanic` | drag-merge |
| `starting_items` | 2× Stone T1, 2× Wood T1 |
| `moves / time` | 10 moves |
| `reward` | 20 coin + 1 hint |
| `intro_affordance` | pet dialog: "Iki taş birleştir!" |
| `outro_affordance` | sandık aç + confetti particle |
| `unlock_next` | otomatik (quest complete) |
| `target_first_try` | 90% |
| `visual_palette` | Tropik Orman pal (#1F7A6C,#F5A623,#C6E0C0) |
| `audio_cue` | ambient_forest_soft.ogg + merge_pop_01.wav |

### Stage 2 — ...
(tüm stage'ler aynı şablonda; runtime'da stage data JSON olarak `Resources/Raw/stages.json`)

---

## 4. Stage geçiş (transition) sistemi

### Normal geçiş (stage → stage)
1. Son merge → pop + sparkle particle burst
2. Quest complete panel slide-in (250ms cubic-out)
3. Reward chest tap → open animation (600ms)
4. Reward item'lar floating to HUD (stagger 80ms)
5. "Sıradaki: Stage 2" button — tap → fade out + next stage fade in (400ms)

### World geçişi (son stage → sonraki world)
1. Stage boss complete → celebration VFX (fireworks + screen shake)
2. Cinematic intro: 3–5 saniye narrative paneli (pet + text)
3. Yeni world haritası reveal animation (flood-fill dissolve)
4. Teaser: "Sahil Mağarası açıldı!" ses + haptic
5. İlk stage otomatik yüklenir VEYA oyuncu "Başla" butonu

### Boss stage unique
- Intro: 5 saniye dramatic (dark lighting, pet endişeli)
- Gameplay: unique obstacle (örn. sınırlı hamle + kayaların üstünde hedef)
- Outro: büyük kutlama + world unlock + rare reward + milestone achievement

---

## 5. Content cadence (live-ops)

| Periyot | Ekleme |
|---|---|
| Launch | 3 world × 20 stage = 60 stage (minimum MVP 60) |
| v1.1 (2 hafta sonra) | +15 stage (World 3 devamı) + 1 event stage |
| v1.2 (4 hafta) | +15 stage (World 4) + mini battle pass (4 hafta) |
| v1.3 (6 hafta) | +20 stage (World 5) + collab event |
| Aylık sabit | +1 event stage (limited-time) + seasonal palette |

---

## 6. Stage authoring tool
- **Format**: JSON (`Resources/Raw/stages.json`), runtime load
- **Editor**: şu an manuel JSON — v1.1'de basit stage editor (MAUI-based admin panel)
- **Versioning**: her stage `version` field'ı; Storage migration log'a yazar

---

## 7. Playtest metodolojisi (pre-ship ve live)
Pre-ship:
- Her stage en az 3 iç playtester
- Completion rate, attempt count, churn point kaydı
- Target first-try sapması > ±15% → rebalance
Live:
- Analytics event: `stage_start`, `stage_complete`, `stage_fail`, `stage_retry_count`, `stage_bailout`
- Haftalık rapor: top-5 churn stage → Level Designer rebalance

---

## 8. Unlock şartları (genel)
- Lineer: quest complete → otomatik next stage
- Gate: world geçişinde "her world'ün tüm stage'lerini bitir" veya "milestone stage'i başar"
- Opt-in: event stage limited-time, notification push ile bildiri
- Purchase gate YASAK (Play policy + player trust)

---

## 9. İmza / onay
- **Level Designer**: ...
- **Game Designer onay**: ...
- **PM onay**: ...
- **Tarih**: {{YYYY-MM-DD}}

---

## Notlar
- Stage sayısı launch'tan sonra live-ops ile büyütülür; MVP minimum 60 stage olsun
- Difficulty curve sapması acceptable ±10%; >±15% rebalance zorunlu
- Her stage QA matrisinde oynanır; P0 bug = stage cut veya fix
- Stage-plan.md Design kapısı zorunlu çıktısıdır; yoksa Build kapısı açılmaz
