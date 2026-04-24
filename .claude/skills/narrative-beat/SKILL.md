---
name: narrative-beat
description: Narrative Designer'ın Narrative kapısında izlediği playbook. Story arc + karakter voice + quest metin bankası.
---

# Skill: narrative-beat

## Ne zaman
Design + Stage Plan sonrası. Narrative-light oyunlar için SKIP edilir (log_append).

## Ön koşul
- brief.md + design.md + stage-plan.md + art-bible.md okundu
- `.claude/rules/narrative.md` okundu

## Adımlar

### 1. World bible (200 kelime)
Setting özeti: ada, lost civilization, antik medeniyet, vb.

### 2. Karakter sheet (her ana karakter)
- Name, age, species, role (protag/sidekick/pet/antagonist)
- **Voice**: tone (cheerful/wise/sassy/shy/stoic)
- Vocabulary level: simple / moderate / sophisticated
- Catchphrase (opsiyonel, 1 kısa ifade)
- Motivation: ne istiyor, ne engel
- Visual anchor: art-bible portrait link

### 3. Story arc (3-5 beat)
1. **Opening hook** — stage 1-5 giriş (inciting incident)
2. **Rising action** — world 2-3 (komplikasyon)
3. **Midpoint twist** — world 3-4 boss (revelation)
4. **Climax** — son world boss (max tension)
5. **Resolution** — outro (reward + world state)

### 4. Quest metin bankası (stage-plan ile senkron)
Her stage için:
- Intro (1 cümle, ≤80 char): objective + hint + character voice
- Outro (1 cümle, ≤80 char): reward mention + reaction + tease
- 2 hint (light + strong)

### 5. Pet dialog bank
- Idle barks: 4-6 line, ≤60 char
- Celebration: 3-4 line
- Failure comfort: 3-4 line

### 6. Boss taunt + defeat
Her boss için:
- 2 taunt (intro + mid-fight)
- 1 defeat line

### 7. Outro / credits
Oyun bitişi — 3-5 line + teaser v1.1

### 8. Localization notes
Culturally specific öğeleri işaretle (Ramazan TR, Sakura JP, Diwali IN).

### 9. Dialog style compliance
- ≤80 char per line
- Karakter voice kilitli
- PEGI 7 uyumlu (no şiddet, küfür, cinsellik)
- No exposition dump

### 10. narrative.md yaz
`games/<id>/narrative.md` — 9 bölüm.
Uzunluk: 400-800 kelime.

## Kapanış
```
artifact_register(gameId, gate="narrative", kind="notes", path="games/<id>/narrative.md")
message_send(to="project-manager", type="handoff", subject="narrative hazır", body="<karakter + arc + dialog count>")
log_append(agent="narrative-designer", gate="narrative", gameId=<id>, decision="<voice + arc>", why="<genre uyumu>")
```

## Yasaklar
- Uzun cutscene (≥5s)
- Text-wall (>150 char popup)
- Political/religious referans
- Gore/violence/sexual (PEGI 7 hedef)
- Grammar hataları

## Done
- narrative.md tam
- Karakter voice + dialog örnek
- Quest bankası stage count ile eşleşti
- Localization notları
- 1 log_append
