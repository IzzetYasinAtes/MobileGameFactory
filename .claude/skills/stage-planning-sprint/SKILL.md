---
name: stage-planning-sprint
description: Level Designer'ın stage planning kapısında izlediği playbook. Brief + design.md'den hareketle stage-plan.md + stages.json üretir. ZORUNLU gate, Build kapısına geçmeden tamamlanır.
---

# Skill: stage-planning-sprint

## Ne zaman
Game Designer design.md'i teslim ettiğinde. PM Level Designer'ı çağırır.

## Ön koşul
- `game_get(gameId)` + `artifact_list(gameId)` → brief + market + design + art-bible
- `.claude/rules/level-design.md` okundu
- `templates/stage-plan.md` template hazır

## Adımlar

### 1. Genre + scale belirleme
brief.md + design.md'den genre tespit:
- match-3 live-ops → launch 100-120 stage, 3-5 world
- merge live-ops → 50 stage, 3 world (MVP)
- hyper-casual → 30-50 stage, 1 world
- idle → 10-15 world + prestige
- premium puzzle → 15-25 stage

### 2. World (biome) listesi
3-5 world öner. Her world için:
- Ad + tema
- Mekanik twist (önceki world'e göre fark)
- Visual palette (3 hex — art-bible'dan)
- Stage sayısı (world başına dağıtım)

### 3. Difficulty curve (her world)
Tier dağılımı:
- tutorial (ilk 3-5): %85+ first-try
- normal (orta %50): %55-75
- hard (sonraki %25): %30-45
- super-hard (%10): %20-30
- boss (son 1): %15-25

Flow zone: 3+ hard ardışık yok.

### 4. Stage listesi
Her stage için 15 alan:
1. stage_id (world-num)
2. world
3. tier
4. variant (mekanik varyant)
5. objective
6. new_mechanic
7. starting_items / board_state
8. moves / time limit
9. reward (coin / XP / hint)
10. intro_affordance (dialog / popup)
11. outro_affordance (reward animation)
12. unlock_next (auto / gated)
13. target_first_try %
14. visual_palette
15. audio_cue

MVP: min 60 stage; prototype: min 10 stage.

### 5. Stage geçişi standardı
`.claude/rules/level-design.md` transition rules:
- Normal: quest complete → reward chest → HUD float → next
- World: boss complete → celebration → narrative → map reveal
- Boss stage: dramatic intro + unique obstacle + big celebration

### 6. Content cadence (live-ops)
| Periyot | Ekleme |
|---|---|
| Launch | 60 stage min |
| v1.1 (+2h) | +15 stage + 1 event |
| v1.2 (+4h) | +15 stage + battle pass |
| v1.3 (+6h) | +20 stage + collab |
| Aylık | +1 event + palette |

### 7. stage-plan.md yaz
`games/<id>/stage-plan.md` — templates/stage-plan.md kopyalanıp doldurulur.

### 8. stages.json üret
`games/<id>/src/<id>/Resources/Raw/stages.json` — runtime load edilebilir format.

### 9. Difficulty curve chart
ASCII tablo: world × tier × expected first-try.

## Kapanış
```
artifact_register(gameId, gate="design", kind="notes", path="games/<id>/stage-plan.md")
artifact_register(gameId, gate="design", kind="notes", path="games/<id>/src/<id>/Resources/Raw/stages.json")
message_send(to="project-manager", type="handoff", subject="stage-plan hazır", body="<world × stage + 1 risk>")
log_append(agent="level-designer", gate="design", gameId=<id>, decision="<stage/world özeti>", why="<curve gerekçesi>")
```

## Yasaklar
- Paywall gate
- "TBD" field
- Difficulty jump >±20% ardışık
- Boss her 10 stage'de bir
- 900 kelime stage description (JSON'a taşı)

## Done
- stage-plan.md tam (15 alan × N stage)
- stages.json geçerli
- Difficulty curve chart
- PM handoff + 1 log
