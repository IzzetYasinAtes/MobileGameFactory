---
name: sound-design-brief
description: Sound Designer'ın Sound kapısında izlediği playbook. SFX palette + music + ambient + mixing.
---

# Skill: sound-design-brief

## Ne zaman
Asset sonrası, Build öncesi. Sound Designer paralel çalışabilir.

## Ön koşul
- design.md (Juice Budget sound sütunu) + stage-plan.md (world bazında ambient) + art-bible.md okundu
- `.claude/rules/sound.md` okundu

## Adımlar

### 1. Music tracks (min 2, max 5)
- `music_menu.ogg` (yumuşak, invite)
- `music_gameplay.ogg` (enerjik ama arka plan)
- `music_boss.ogg` (tense, opsiyonel)
- `music_event.ogg` (festive, opsiyonel)
- `music_victory.ogg` (celebrate, opsiyonel)

Format: OGG 128 kbps stereo, seamless loop (fade-in 2s, fade-out 2s), 90-180s.

### 2. SFX palette (minimum)

#### UI
btn_tap, btn_hover, toast_in, popup_open, popup_close, tab_switch

#### Gameplay
merge_pop (3 varyant, pitch ±10%), match_chain_1/2/3 (semitone ladder), coin_pickup, item_drop, hint_ping

#### Feedback
success_fanfare, fail_buzz, reward_tada, level_up_chime, unlock_chime

#### Ambient (world-specific)
world_1_forest_bed, world_2_beach_bed, world_3_temple_bed, ...

Format: OGG 64 kbps, ≤1 MB per file.

### 3. Mixing rules (LUFS)
- Music: -18
- SFX: -12
- Ambient: -24
- Voice: -14

Ducking: voice > SFX > music (voice play → music -40% 300ms fade).

### 4. Pitch variance
Tekrar eden SFX'de `PlaybackRate` ±10% random. Combo chain semitone ladder (+1 per step).

### 5. Layering
Karmaşık event için 2 layer: attack (20-50ms transient) + body (200-500ms tone).

### 6. AI / pipeline tools
- Music gen: Suno / Udio / Soundraw (royalty-free ticari check)
- SFX gen: ElevenLabs / Freesound (CC0) / AudioJungle
- Edit: Audacity / REAPER / Wavosaur

### 7. MAUI integration
```csharp
IAudioManager audioManager;
IAudioPlayer player = audioManager.CreatePlayer(FileSystem.OpenAppPackageFileAsync("merge_pop.ogg").Result);
player.PlaybackRate = Random.Shared.NextDouble() * 0.2 + 0.9;
player.Play();
```

Resource path: `games/<id>/src/<id>/Resources/Raw/sfx/` ve `Resources/Raw/music/`.

### 8. Preload critical
App startup: btn_tap, merge_pop, coin_pickup preload.

### 9. Accessibility
- Music + SFX separate slider 0-100
- Haptic opt-out
- Visual cue substitution (hearing-impaired)

### 10. sound-brief.md yaz
`games/<id>/sound-brief.md` — 9 bölüm + SFX/music listesi.
Uzunluk: 400-600 kelime.

## Kapanış
```
artifact_register(gameId, gate="sound", kind="notes", path="games/<id>/sound-brief.md")
artifact_register(gameId, gate="sound", kind="asset", path="games/<id>/assets/sfx/")
artifact_register(gameId, gate="sound", kind="asset", path="games/<id>/assets/music/")
message_send(to="project-manager", type="handoff", subject="sound hazır", body="<music + SFX count + size>")
log_append(agent="sound-designer", gate="sound", gameId=<id>, decision="<palette + mixing>", why="<juice uyumu>")
```

## Yasaklar
- SFX >1 MB
- Music >5 MB
- Lisanslı kaynak attribution'suz
- Headphone-only mix
- Seizure-trigger ses

## Done
- sound-brief.md tam
- SFX placeholder oyunda hook
- Music 2+ track ready
- Sound opt-out toggle
- 1 log_append
