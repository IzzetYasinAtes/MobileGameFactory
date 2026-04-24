---
name: kill
description: Bir oyunu öldürür. POSTMORTEM yazılır, status=killed, git branch arşiv.
---

# /kill <game-id>

Sahip bir oyunu öldürmeye karar verdiğinde veya Polish-or-Kill gate'te kill çıktığında kullanılır.

## Adımlar
1. `game_get(id)` — mevcut durum
2. `game_meta_patch(id, {"status":"killed"})`
3. `games/<id>/POSTMORTEM.md` yaz:
   - Vizyon (brief'teki hedef)
   - Teslim edilen
   - Kill kararı tarihi + 5 soru sonucu + net neden
   - Öğrenilen insight (3 madde)
   - Sistem feedback (agent/rule/skill güncelleme tetikler mi?)
   - Aksiyon öğeleri
4. `log_append(agent="project-manager", gate=<mevcut>, decision="KILL", why=<net neden>)`
5. Git branch `game/<id>` korunur (tarihsel kayıt, silinmez)
6. Sahibe kısa özet: "Oyun X öldürüldü, sebep Y, Z kaynak harcandı, öğrendiğimiz A."

## Kill disiplini
Supercell/Voodoo pattern: çoğu oyun öldürülür, azı ship'e gider. Kill utanç değildir — sistem gücüdür.
