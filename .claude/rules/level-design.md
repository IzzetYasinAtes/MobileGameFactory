# Level Design Kuralları

## Kural #1: Stage-plan.md Design kapısı çıktısıdır, zorunludur
Design kapısı kapanmadan Build kapısı **açılmaz**. PM bu doğrulamayı yapar.

## Minimum stage kadranı (launch)
Tür bazında mutlak minimum (aşağısı "oyun değil, demo"):

| Genre | Min stage | Min world/chapter | Notu |
|---|---|---|---|
| Match-3 live-ops | 60 | 3 | v1.0 minimum; aylık +15 ile büyür |
| Merge live-ops | 50 | 3 | board progression + biome değişimi |
| Hyper-casual | 30 | 1 | tek mekanik yeterli |
| Idle / Clicker | 10 | 10 world + prestige | world yerine "stage" kullanılmaz |
| Premium puzzle | 15 | 3 | daha az ama zor |
| Adventure / story | 20 | 3–5 chapter | narrative ağırlıklı |

## Difficulty curve hedefleri

Her world için bu dağılım korunur:

| Tier | % of world stages | Target first-try rate |
|---|---|---|
| tutorial | ilk 3–5 | %85+ |
| normal | orta %50 | %55–75 |
| hard | sonraki %25 | %30–45 |
| super-hard | son %10 | %20–30 |
| boss | world'ün son stage'i | %15–25 |

Sapma:
- Sapma ±10% acceptable
- Sapma >±15% → Level Designer rebalance zorunlu
- Stage **çok kolay** (first-try >%90 normal tier'da) → daha zor mekanik ekle
- Stage **çok zor** (first-try <%20 hard tier'da) → quota/hamle genişlet

## Pacing kuralları

- **Flow zone**: ardışık 3+ "hard" stage yok; 2 hard sonra 1 "easy relief" stage
- **Tension curve**: her world easy → normal → hard → super-hard → boss yükselir
- **Rest level**: boss sonrası 1–2 easy stage (oyuncu nefes alır)
- **Milestone her 25–50 stage**: unique visual + narrative beat

## Stage-plan.md zorunlu alanlar (template'ten)
`templates/stage-plan.md` kopyalanır, her field doldurulur. Eksik field = design kapısı reddi.

Alanlar:
1. `stage_id` (örn: 1-05 = world 1 stage 5)
2. `world` (biome/chapter)
3. `tier` (tutorial/normal/hard/super-hard/boss)
4. `variant` (mekanik varyant)
5. `objective` (görev açıklaması)
6. `new_mechanic` (bu stage'de tanıtılan yeni mekanik varsa)
7. `starting_items` / `board_state`
8. `moves / time` (limit)
9. `reward` (coin/XP/hint/item)
10. `intro_affordance` (dialog/cutscene/popup)
11. `outro_affordance` (reward animation)
12. `unlock_next` (otomatik/gated)
13. `target_first_try` (%)
14. `visual_palette` (bg + 3 hex)
15. `audio_cue` (ambient + key SFX)

## Stage geçişi (transition) standardı

### Stage → Stage
1. Quest complete panel slide-in (250ms cubic-out)
2. Reward chest tap → open animation (600ms)
3. Reward floating to HUD (stagger 80ms per item)
4. "Sıradaki" button → fade out 400ms + next stage fade in 400ms
5. Ara ekran yok — kesintisiz akış

### World → World
1. Boss complete → 2–3 saniye celebration (fireworks + shake)
2. Narrative panel (pet + text + portrait, 3–5 saniye)
3. World map reveal (flood-fill 800ms)
4. Teaser + SFX (unlock_chime + haptic)
5. Otomatik next world veya "Başla" button

### Boss stage unique
- Pre: 3 saniye dramatic intro (ışık değişir, pet endişeli)
- During: unique obstacle
- Post: büyük kutlama + rare reward + milestone achievement unlock

## Authoring format

**JSON-based stage data** — `Resources/Raw/stages.json`:
```json
{
  "version": 1,
  "stages": [
    {
      "id": "1-01",
      "world": 1,
      "tier": "tutorial",
      "variant": "2-match-teach",
      "objective": { "type": "merge_count", "chain": "Stone", "tier": 2, "quantity": 2 },
      "starting_items": [
        { "chain": "Stone", "tier": 1, "cell": 0 },
        { "chain": "Stone", "tier": 1, "cell": 1 }
      ],
      "moves": 10,
      "time": null,
      "reward": { "coin": 20, "hint": 1 },
      "intro_dialog": "pet.kasif.intro_1_01",
      "target_first_try": 0.9,
      "palette": "world_1_tropical"
    }
  ]
}
```

## Playtest metodolojisi

### Pre-ship
- Her stage **en az 3 iç playtester** (farklı skill seviyeleri)
- Kayıt: completion rate, attempt count, churn point, fun rating (1–5)
- Dashboard: `games/<id>/stage-metrics.md`
- Target sapması >±15% → rebalance

### Post-ship (live)
- Analytics events: `stage_start`, `stage_complete`, `stage_fail`, `stage_retry_count`, `stage_bailout`
- Haftalık rapor: top-5 churn stage → Level Designer rebalance queue
- Dashboard: GameAnalytics/Firebase/Amplitude

## Yasaklar
- Paywall gate (stage unlock için IAP zorunluluğu)
- Energy block tutorial'da (L1–15 energy cooldown=0)
- Unreachable stage (bug sonucu geçilemeyen stage)
- Grind wall (aynı stage'i N kez geçmeden ilerleme yok)
- Dark pattern: sahte "son saat!" timer
