# Sound Design Kuralları

## Sert kurallar
1. **Her juice event min 3 kanaldan** — sound zorunlu kanal
2. **Total audio size ≤5 MB** (AAB budget korunur)
3. **Format**: OGG Vorbis, ≤128 kbps music, ≤64 kbps SFX
4. **LUFS mixing**: music -18, SFX -12, ambient -24
5. **Ducking**: voice > SFX > music
6. **Pitch variance ±10%** tekrar eden seste (monotonluk kırılır)

## SFX palette (minimum set)

### UI
- `btn_tap.ogg` — primary button
- `btn_hover.ogg` — hover (desktop/tablet)
- `toast_in.ogg` — toast appear
- `popup_open.ogg` — modal açılış
- `popup_close.ogg` — modal kapanış
- `tab_switch.ogg` — tab transition

### Gameplay (core loop)
- `merge_pop.ogg` (3 varyant, pitch ±10%) — merge başarılı
- `match_chain_1/2/3.ogg` — combo chain (semitone ladder)
- `coin_pickup.ogg` (randomized pitch) — coin collect
- `item_drop.ogg` — item spawn
- `hint_ping.ogg` — hint tap

### Feedback
- `success_fanfare.ogg` — quest complete
- `fail_buzz.ogg` — level fail
- `reward_tada.ogg` — chest open
- `level_up_chime.ogg` — level up
- `unlock_chime.ogg` — world unlock

### Ambient (world-specific)
- `world_1_forest_bed.ogg` — loop 90-180s
- `world_2_beach_bed.ogg`
- `world_3_temple_bed.ogg`
- ...

### Voice (opsiyonel)
- `pet_kasif_yes.ogg`
- `pet_kasif_happy.ogg`
- `pet_momo_chuckle.ogg`

## Music tracks (min 2, max 5)

### Minimum
1. `music_menu.ogg` — main menu loop
2. `music_gameplay.ogg` — core loop ambient

### İdeal
1. Menu (yumuşak, invite)
2. Gameplay normal (enerjik ama arka plan)
3. Boss / super-hard (tense)
4. Event (festive)
5. Victory / ending (celebrate)

### Format kuralları
- OGG Vorbis, 128 kbps stereo
- Seamless loop (fade in 2s, fade out 2s, cross-fade point marked)
- Duration: 90–180s loop (çok kısa loop sıkar)
- Key: minor casual relaxing, major uplift event/win

## Mixing rules (LUFS)

| Kanal | Target LUFS | Notu |
|---|---|---|
| Music | -18 | Arka plan, hiç öne çıkmaz |
| SFX | -12 | Music üstünde duyulur |
| Ambient | -24 | Arka arka plan, bg noise |
| Voice | -14 | SFX ile aynı, music üstünde |

### Ducking
Voice play → music -40% (300ms fade), voice bitince music restore.
SFX play → music hafif -10% dip 150ms (opsiyonel, yoğun sahne).

## Pitch variance (monotonluk önler)
- Tekrar eden SFX'de `PlaybackRate` ±10% randomize
- Combo chain'de semitone ladder:
  - Chain 1: 0 semitone
  - Chain 2: +1 semitone (1.0595)
  - Chain 3: +2 semitone (1.1224)
  - Chain 4+: +3 semitone (1.1892)
- Matematik: `rate = 2^(semitones/12)`

## Layering
Karmaşık event (ör: merge pop) 2 layer:
- **Attack** (transient 20-50ms): `merge_attack.ogg`
- **Body** (tone 200-500ms): `merge_body.ogg`
- İkisi birlikte çalar (simultaneous)

## AI / pipeline tools

### Music generation
- **Suno** — vocal + full track, ticari lisans check
- **Udio** — high quality, ticari lisans check
- **Soundraw** — royalty-free, genre/mood filter
- **Riffusion** — open source, local

### SFX generation
- **ElevenLabs Sound Effects** — text-to-SFX
- **Freesound** — CC0 + attribution library
- **AudioJungle** — ticari, $1-5/SFX
- **99Sounds** — free pack'ler

### Manuel edit
- **Audacity** (ücretsiz) — trim, normalize, fade
- **REAPER** ($60 bir kerelik) — pro mixing
- **Wavosaur** — seamless loop

## MAUI integration

```csharp
// Plugin.Maui.Audio
using Plugin.Maui.Audio;

IAudioManager audioManager;
IAudioPlayer player = audioManager.CreatePlayer(FileSystem.OpenAppPackageFileAsync("merge_pop.ogg").Result);
player.PlaybackRate = Random.NextDouble() * 0.2 + 0.9; // 0.9 - 1.1
player.Play();
```

### Resource path
`games/<id>/src/<id>/Resources/Raw/sfx/<name>.ogg`
`games/<id>/src/<id>/Resources/Raw/music/<name>.ogg`

### Preload critical SFX
App startup'ta preload:
- `btn_tap.ogg`
- `merge_pop.ogg`
- `coin_pickup.ogg`

### Cross-fade music
Scene transition'da 1s cross-fade:
```csharp
currentPlayer.FadeOut(1000);
nextPlayer.FadeIn(1000);
```

## Accessibility
- **Sound opt-out**: Settings → music slider 0-100, SFX slider 0-100 ayrı
- **Haptic opt-out**: Settings toggle
- **Visual substitution**: sesli cue'lar görsel indicator ile desteklenir (hearing-impaired)

## QA checklist
- [ ] Tüm SFX <1 MB
- [ ] Tüm music <5 MB
- [ ] Toplam audio <5 MB
- [ ] LUFS hedefleri tutuyor
- [ ] Telefon speaker'da test edildi (not just headphone)
- [ ] Pitch variance çalışıyor (monotonluk yok)
- [ ] Ducking voice > SFX > music doğru
- [ ] Sound opt-out toggle çalışıyor
- [ ] Music loop seamless (click yok)

## Yasaklar
- SFX > 1 MB tek dosya
- Music track > 5 MB
- Lisanslı kaynak attribution olmadan
- Headphone-only mix (telefon speaker unutulmaz)
- Seizure-trigger ses (3 Hz altı pulse)
- Auto-play music app inactive'de
