# Sound Brief — {{title}}

**ID**: `{{id}}`
**Sound Designer**: {{name}}
**Tarih**: {{YYYY-MM-DD}}
**Kapı**: sound

---

## 1. Music tracks
| # | Dosya | Kullanım | Duration | Mood |
|---|---|---|---|---|
| 1 | music_menu.ogg | Main menu | 120s loop | Yumuşak, invite |
| 2 | music_gameplay.ogg | Core loop | 150s loop | Enerjik arka plan |
| 3 | music_boss.ogg | Boss stage | 90s loop | Tense |
| 4 | music_event.ogg | Event | 120s loop | Festive |
| 5 | music_victory.ogg | Win | 15s short | Celebrate |

**Format**: OGG 128 kbps stereo, fade-in 2s fade-out 2s, seamless loop.

## 2. SFX palette

### UI
- btn_tap.ogg
- btn_hover.ogg
- toast_in.ogg
- popup_open.ogg
- popup_close.ogg
- tab_switch.ogg

### Gameplay
- merge_pop.ogg (3 varyant, pitch ±10%)
- match_chain_1/2/3.ogg (semitone ladder)
- coin_pickup.ogg (randomized pitch)
- item_drop.ogg
- hint_ping.ogg

### Feedback
- success_fanfare.ogg
- fail_buzz.ogg
- reward_tada.ogg
- level_up_chime.ogg
- unlock_chime.ogg

### Ambient (world-specific)
- world_1_forest_bed.ogg
- world_2_beach_bed.ogg
- world_3_temple_bed.ogg
- world_4_volcano_bed.ogg
- world_5_ice_bed.ogg

### Voice (opsiyonel)
- pet_kasif_yes.ogg
- pet_kasif_happy.ogg

**Format**: OGG 64 kbps, ≤1 MB per file.

## 3. Mixing (LUFS)
- Music: -18 (arka plan)
- SFX: -12 (music üstünde)
- Ambient: -24 (bg noise)
- Voice: -14 (SFX ile eşit)

**Ducking**: voice > SFX > music. Voice play → music -40% 300ms fade.

## 4. Pitch variance
- Tekrar eden SFX: `PlaybackRate` ±10% random
- Combo chain: semitone ladder (+1, +2, +3)
- Matematik: `rate = MathF.Pow(2, semitones / 12f)`

## 5. Layering
Karmaşık event: **attack** (20-50ms transient) + **body** (200-500ms tone) simultaneous.

## 6. Pipeline
- Music gen: Suno / Udio / Soundraw
- SFX gen: ElevenLabs / Freesound CC0 / AudioJungle
- Edit: Audacity / REAPER / Wavosaur (seamless loop)

## 7. Budget
Toplam audio ≤5 MB (AAB <40 MB hedef).

## 8. MAUI integration
```csharp
IAudioManager audioManager;
IAudioPlayer player = audioManager.CreatePlayer(
    FileSystem.OpenAppPackageFileAsync("merge_pop.ogg").Result);
player.PlaybackRate = Random.Shared.NextDouble() * 0.2 + 0.9;
player.Play();
```

Resource: `games/<id>/src/<id>/Resources/Raw/sfx/` ve `Resources/Raw/music/`.

## 9. Preload
App startup: btn_tap, merge_pop, coin_pickup preload.

## 10. Accessibility
- Music slider 0-100
- SFX slider 0-100 (ayrı)
- Haptic toggle
- Visual substitution (hearing-impaired)
