---
name: liveops-event-plan
description: LiveOps Manager'ın LiveOps kapısında izlediği playbook. Daily + weekly + battle pass + push cadence.
---

# Skill: liveops-event-plan

## Ne zaman
QA + Analytics sonrası, Release öncesi. LiveOps Manager yürütür.

## Ön koşul
- design.md, stage-plan.md, monetization.md, analytics.md okundu
- `.claude/rules/live-ops.md` okundu

## Adımlar

### 1. Daily rotation (7 gün)
Her gün farklı quest tipi:
- Pzt: match quest
- Sal: combo quest
- Çar: time attack
- Per: puzzle variant
- Cum: event preview
- C-P: weekend bonus

### 2. Weekly event takvimi (12 hafta önden)
Her hafta:
- 1 tournament (7 gün leaderboard, top 100 reward)
- 1 mini event (72 saat, unique objective + reward)
- Weekend IAP bundle (Cuma-Pazar %20-40 discount 1 SKU)

### 3. Mini battle pass (4 hafta cycle)
- 30 tier free + 30 tier premium ($4.99)
- Free: coin, hint, boost
- Premium: character skin, event-exclusive, epic reward tier 30
- XP: stage complete +10, daily quest +20-50, event +100

### 4. Seasonal palette (aylık)
Spring/Summer/Fall/Winter palette shift + hero art swap.

### 5. Push notification template
Max 2/gün, 9-21 local time. Segment × timing × message:
- Non-payer 24h inactive: "Enerjin doldu! Macera bekliyor"
- Payer 48h inactive: "Yeni event başladı!"
- Churned 7d+: "Seni özledik! Yeni bölge açıldı"
- Event start: "Volkanik Event - 72 saat!"
- Streak: "Streak devam! Bugün 5. gün"

### 6. LTO stratejisi
- Starter Pack: launch + 24h (2 pencere)
- Weekend Bundle: Cuma 18:00 - Pazar 23:59
- Post-loss offer: 3 fail arka arkaya → 1 ucuz boost paketi (75% discount)
- Whale offer: LTV >$100 monthly custom bundle

### 7. KPI monitoring
Haftalık rapor: D1/D7/ARPDAU/payer/ROAS/crash-free/session_length/sessions_dau.

Alarm <target: incele, rollback `log_append(decision="rollback")`.

### 8. liveops-calendar.md yaz
`games/<id>/liveops-calendar.md` — 7 bölüm + 4 hafta event queue + push template.
Uzunluk: 600-900 kelime.

## Kapanış
```
artifact_register(gameId, gate="liveops", kind="notes", path="games/<id>/liveops-calendar.md")
message_send(to="project-manager", type="handoff", subject="liveops hazır", body="<event queue + battle pass>")
log_append(agent="liveops-manager", gate="liveops", gameId=<id>, decision="<cadence>", why="<metric + ops yükü>")
```

## Yasaklar
- Push spam >2/gün
- Fake "son 1 saat!" timer
- Pay-to-progress event
- Weekend event-free
- Event content post-ship

## Done
- liveops-calendar.md tam
- 12 hafta event queue
- Battle pass 30+30 tier
- Push template
- 1 log_append
