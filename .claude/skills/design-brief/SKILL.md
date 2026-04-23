---
name: design-brief
description: Game Designer agent'ın design kapısında izlediği playbook. market.md + brief.md'den hareketle tek oturumda design.md üretir.
---

# Skill: design-brief

## Ön koşul
- `game_get(gameId)` + `artifact_list(gameId)` → brief.md ve market.md path'leri.
- Her ikisi de okundu.

## Adımlar

### 1. Core loop netleştir
Sorular (kendine yanıtla, karar ver):
- Oyuncu ne yapar? (tek parmak tap / tilt / swipe)
- Bir turunda ne zaman "heyecan piki" olur?
- Kaybetme/bitiş nasıl? Instant restart ne kadar hızlı?
- Hedef session süresi: 60–180 s.

Kararı 1 cümlede yaz.

### 2. Progression
- Unlock mekaniği (karakter/tema/skill).
- Currency: soft (grind ile) + hard (IAP).
- Progress pacing: D1/D3/D7'de oyuncu nerde olmalı.

### 3. Difficulty modeli
- Dinamik mi (DDA) yoksa eğri mi?
- Giriş formülü (skor, zaman, hata sayısına göre).
- Örnek sayılar: level 1, level 10, level 50.

### 4. Monetization noktaları (design perspektifi)
Her noktayı bir cümlede tanımla:
- **Rewarded**: ne teklif? (revive / 2x / skip)
- **Interstitial** yerleşim: hangi geçişte (core loop dışı)?
- **IAP**: ne tür (remove-ads / cosmetic / pack)?

Detay ve denge: Monetization agent yapacak; burada sadece iskelet.

### 5. Retention hook
D1, D3, D7 için ayrı kanca (örn: daily challenge, streak bonus, yeni unlock eşiği). Her kanca 1 cümle.

### 6. Ses / görsel yön
3 madde: palet, müzik tonu, SFX teması. Asset agent/bedevasuz yok — MAUI developer + sahibin tercihleri belirler.

### 7. Teknik uyarılar
MAUI özelinde bilinmesi gerekenler:
- Hangi kontrol (GraphicsView/Skia)?
- Veri modelinin SQLite şeması (tablo başlıkları).
- Platform-özgü API ihtiyacı (vibration, keyboard, vb).

### 8. design.md yaz
Path: `docs/games/<id>/design.md` — `templates/design-doc.md` şablonundan.
Uzunluk 600–900 kelime.

### 9. Kapı kapanış
```
artifact_register(gameId, gate="design", kind="design", path="docs/games/<id>/design.md")
message_send(to="project-manager", type="handoff", subject="design.md hazır", body="<core + 1 risk>")
log_append(agent="game-designer", gate="design", gameId=<id>, decision="<core loop>", why="<ana seçim>")
```

## Yasaklar
- 2 core loop versiyonu ("ya da şöyle"). Tek karar.
- Açık soru sayısı 2'den fazla (PM karar verecek; sen önce öner).
- 900 kelime üstü.

## Done
design.md kayıtlı + PM handoff + 1 log.
