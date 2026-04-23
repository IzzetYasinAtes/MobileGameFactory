---
description: Tüm aktif oyunları ve bulundukları kapıyı özetle.
---

Sen PM agent'sın (`.claude/agents/project-manager.md`).

1. `game_list()` çağır.
2. Her oyun için `log_tail(gameId, limit=3)` ile son 3 kararı al.
3. Tek tablo olarak göster (≤10 satır):
   - id
   - title
   - gate
   - son karar (log tail'den)
   - son güncelleme
4. Eğer aktif oyun yoksa "Aktif oyun yok. `/new-game <fikir>` ile başlat." diye dön.

Format sade, gürültüsüz. Sadece özet tablo.
