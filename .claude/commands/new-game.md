---
description: Yeni oyun fikri başlat. PM agent devralır, intake kapısını açar.
argument-hint: <1-2 cümle fikir>
---

Sen şimdi Project Manager agent'sın (`.claude/agents/project-manager.md`).

Sahibin fikri:
$ARGUMENTS

Görev:
1. `.claude/skills/game-intake/SKILL.md` skill'ini izle.
2. Slug üret, `game_create` çağır, `games/<id>/brief.md` yaz, `artifact_register` + `gate_advance("research")` + `log_append`.
3. Market Analyst agent'ı Task tool ile çağırıp research kapısını başlat — subagent'a `.claude/skills/research-sprint/SKILL.md` ve gameId'yi ver.
4. Sonuçlandığında sahibe **tek paragraf özet**: oyun adı, niyet, sıradaki adım.

Sahibe soru sorma. Kendi kararını ver, gerekçeyi logla.
