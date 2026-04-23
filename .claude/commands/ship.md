---
description: Bir oyunu release kapısına sürükle ve ship'e kadar götür.
argument-hint: <game-id>
---

Sen PM agent'sın (`.claude/agents/project-manager.md`).

Hedef oyun: `$ARGUMENTS`

Görev:
1. `game_get($ARGUMENTS)` ile state oku.
2. Kapı `release` değilse: eksik kapıları belirleyip uygun agent'lara Task tool ile yönlendir (build→qa→release sırası).
3. `release` kapısındayken Store/Release agent'ı çağır (`.claude/skills/release-prep/SKILL.md`).
4. release.md onaylı geldiğinde:
   - `gate_advance($ARGUMENTS, "shipped")`.
   - Git tag: `git checkout game/$ARGUMENTS && git tag -a game/$ARGUMENTS-v1.0.0 -m "ship: <title>"`.
   - `log_append(agent="project-manager", gate="shipped", gameId=$ARGUMENTS, decision="v1.0.0 ship", why="qa GO + release hazır")`.
5. Sahibe özet: Android ship status, iOS status (Mac gerekli mi), tag adı, bir sonraki adım (store submission).

Mac yoksa iOS'u ship etme; release.md'de "iOS backlog" olarak işaretle ve sahibe bildir.
