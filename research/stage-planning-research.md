# Stage / Level Planning Research — Mobil Casual Oyunlar

> Kaynaklar: King (Candy Crush), Dream Games (Royal Match), Playrix (Gardenscapes / Homescapes), Zynga (Merge Dragons), CrazyLabs / Supersonic / Sunday (hyper-casual), Game Developer magazine, Level Design Book, GameAnalytics, devtodev. Tarih: 2026-04-24.

## 1. Tür bazında level sayısı (launch ve live ops)

| Tür | Launch minimum | Launch "polished" | Live-ops kadansı | Referans |
|---|---|---|---|---|
| **Match-3 (Royal Match / Candy tarzı)** | 100 level | 150–200 level | **2 haftada 100 level** (Royal Match), **haftada 15–50** (Candy Crush) | Dream Games, King — bugün 7.000–14.000 level |
| **Merge (Merge Dragons, Gossip Harbor)** | 30–50 level + açık dünya | 80–120 level | 2–4 haftada 1 yeni ada/bölge | Merge Dragons toplam 396 level |
| **Meta-match (Homescapes, Gardenscapes)** | 200 match-3 level + 3–5 oda | 300+ level + 8+ oda | Haftalık oda/alan + 20–40 level | Playrix |
| **Hyper-casual** | 30–50 level, tek mekanik | 80–120 level | Event/kozmetik; level patch nadir | Supersonic, CrazyLabs |
| **Idle / Incremental** | 5–10 "world" + prestige yok | 15–20 world + 1 prestige layer | Ayda 1 yeni prestige katmanı / world | Kongregate math-of-idle serisi |
| **Puzzle (Monument Valley tarzı)** | 10 level | 20–25 level | DLC chapter'ları (yıllık) | Premium pattern |
| **Adventure / Story-driven casual** | 5 chapter × 10 level | 10 chapter × 15 level | Haftalık story beat | Playrix "Scapes" serisi |

**Kural**: live-ops oyunu (match-3, merge, meta-match) launch'a **en az 100–150 level** ile girmeli. İlk 7 gün retention'ı bitirecek içerik tüketim hızı budur. Hyper-casual ve premium puzzle için launch'ta 30–50 level yeterli, live-ops yükü minimum.

## 2. Level count eğrisi — live-ops

- **Royal Match**: Şubat 2021 launch → Nisan 2026'da 7.000+ level. İlk ayda 2 haftada bir 50 level; Temmuz 2021'den itibaren 2 haftada 100 level. Yani yıllık ~2.500 level.
- **Candy Crush Saga**: 2012 launch → 2026'da ~14.000 level. Haftalık Çarşamba drop'u, drop başına 15–45 level. King "kötü" level'ları budar ve yeniden tasarlar (telemetry tabanlı).
- **Homescapes**: haftalık 20–40 match-3 level + 1–2 oda/dekor güncellemesi.

Sahip için kural: **biz KÜÇÜK oyun yapıyoruz** → launch 60–120 level, yıllık live-ops 200–400 level yeterli. Royal Match kadansı AAA-live-ops team (40+ kişi) gerektirir; bizim için hedef değil.

## 3. Stage planning şablonu — zorunlu alanlar

King, Dream Games ve Playrix'in level briefleri şu ortak alanlara sahip:

- **stage_id** (stable, `<game>-stg-0042`)
- **world / biome** (hangi bölge, tematik grup)
- **theme / narrative beat** (tek cümle, karakter/hikaye bağlantısı)
- **visual environment** (bg art slug, palet, prop seti)
- **intro affordance** (cinematic / dialog / loading-tip / instant unlock — dördünden biri)
- **gameplay variant** (standart, boss, bonus, puzzle-twist, rest)
- **objective** (clear jelly, reach score, collect 12 X, survive 60s)
- **difficulty tier** (easy / normal / hard / super-hard / nightmare — King taksonomisi)
- **target metrics**: hedef completion rate (first-try %), hedef attempt count ortalaması
- **new mechanic introduced?** (evet ise: slug + hangi level'dan başlayarak beklenir)
- **reward** (coin, booster, chest, XP, chapter-unlock, story-beat)
- **outro affordance** (animation, reward popup, story cutscene)
- **unlock rule for next** (instant / N-star gate / timer / cinematic wait)
- **audio shift?** (bg music loop id, ambient cue)

Playtest sonrası eklenenler:
- `observed_completion_rate`, `observed_attempts_avg`, `churn_percent`, `fun_rating`, `action` (keep / rebalance / cut)

## 4. Difficulty curve — tension, flow, rest

Tüm big-three (King, Dream, Playrix) aynı prensibi kullanıyor: **macro flow channel + micro oscillation**.

- Oyuncu skor bandına göre zorluğu kademeli yükselir → flow bandı (ne sıkıcı ne panik).
- Her 3–5 level'da bir "easy win" (rest level) → dopamin pompası + şehir turuna soluklanma.
- Her 8–12 level'da bir "tension peak" → hard level, retry gerektirir (ama %60+ completion rate'te kalsın).
- Her 25–50 level'da bir **milestone / boss stage**: unique challenge, özel görsel, büyük ödül chest, yeni world unlock.

Candy Crush'tan ölçüm: King, "time-to-pass" + "time-to-abandon" karışımıyla **fun-metric** üretir. Completion rate %40'ın altına düşen level → "super hard" etiketi, rebalance kuyruğuna gider. %10 altına düşen = kesilir veya yumuşatılır.

Sayısal hedef (küçük oyun için):
- Normal level: first-try completion **%55–75**
- Hard level: first-try **%30–45**, 3 deneme cumulative **%70+**
- Boss level: first-try **%15–25**, 5 deneme cumulative **%60+**
- Herhangi bir level %10 altı = P0 rebalance

## 5. Stage geçiş mantığı

Endüstri envanteri:

| Yaklaşım | Nerede | Ne zaman |
|---|---|---|
| Instant unlock | Hyper-casual, idle | Düşük friction, hızlı loop |
| Cinematic (5–10s) | Meta-match (Homescapes oda reveal) | Milestone, chapter dönüşü |
| Loading-tip | Candy Crush | Standart level → level geçişi |
| Dialog / story beat | Royal Match (King Robert) | Her 5 level'da bir |
| Reward chest | Royal Match, Coin Master | Her level sonu; %100 ödül |
| Map zoom + camera pan | Candy Crush, Royal Match | World bittiğinde (visual payoff) |

Kural: **%80 level → instant unlock + loading tip**, **%15 → reward chest / story beat**, **%5 boss → cinematic + camera pan**. Fazla cinematic = friction; sıfır cinematic = textureless progress.

## 6. Stage görselleri ve audio shift

- **Background art**: world başına 1 ana art + 3–5 varyasyon (gün/gece/hava). World ~25–50 level sürer → art budget amorti.
- **Color story**: world başına 1 dominant palet (Royal Match: her kale odası ayrı palet). Saturation, warmth geçişi retention'ı besler.
- **Prop / obstacle reskin**: mekanik aynı, skin world'e göre (jelly → ice crystal → cursed bone).
- **Character variant**: story beat'te karakter skin değişimi (Royal Match: Kral Robert kıyafeti).
- **Audio shift**: world başına 1 bg loop (2–4 dk), boss level'da **tempo +%20 + key change**.

## 7. Progression UI — map tipi seçimi

| Patern | Örnek | Ne zaman seç |
|---|---|---|
| Doğrusal horizontal/vertical path | Candy Crush, Royal Match | Match-3 live-ops, 100+ level |
| Dünya hub + multi-map | Gardenscapes, Merge Dragons | Meta-progression varsa |
| Chapter kart listesi | Hyper-casual | 20–50 level, düşük friction |
| Grid / odaklı slot | Idle / Incremental | Prestige layer görselleştirme |
| Physical 3D diorama | Monument Valley, Alto serisi | Premium, az level |

**Kural**: 100+ level hedefiyorsan **sürekli scroll patik harita** (Royal Match patern). <30 level için list/grid. Meta-progression varsa world hub.

## 8. Tutorial dağılımı (ship blocker)

- **Level 1–3**: sadece core mekanik. Tutorial pop-up yok, seviye kendini öğretir (intuitive teaching).
- **Level 4–8**: ikinci mekanik / ilk objective variant. Guided hint overlay.
- **Level 9–15**: ilk hard level + rewarded ad ilk defa tanıtılır (revive placement).
- **Level 16–25**: yeni mekanik her 5 level'da bir. Three-phase: beginner → moderate → expert.
- **Level 25**: ilk milestone / mini boss.
- **Level 50**: ilk tam boss + yeni world unlock.

King / Dream data: **interstitial ilk 3 run'da yok, IAP pitch level 8'den önce yok**. ATT / consent flow ilk açılışta (session 1, pre-level-1).

## 9. Level authoring tools — opsiyonlar

Endüstri: **her başarılı match-3 studio'nun kendi editor'u var** (internal tool). MAUI context'inde:

- **JSON-based authoring** (tavsiye): her level `games/<id>/assets/levels/<stage_id>.json`. Shema: objective, grid, obstacles, moves, reward. Game runtime runtime'da deserialize (System.Text.Json source-gen).
- **Unity/Godot hybrid**: küçük oyunlar için overkill. MAUI root'umuz var, ayrı tooling karmaşa yaratır.
- **MCP tool**: `level_register(stage_id, path)` → artifact_register üstüne ince wrapper. Böylece level sayısı ve kapı ilerlemesi MCP'den izlenir.

Tavsiye: **JSON + MAUI içinde basit level preview sayfası** (debug menüsü). Ayrı editor tool yatırımı yok.

## 10. Content cadence (küçük oyun için gerçekçi)

- Launch: 60–120 level, 3–5 world.
- Güncelleme: 2 haftada 1, drop başına 10–20 level + 1 event stage.
- Event stage: milestone stage tipinde, 7 gün açık, unique reward.
- Yıllık hedef: 200–400 yeni level.

---

# Çıktı: `stage-plan.md` template iskelet'i (games/<id>/stage-plan.md)

Bu dosya Game Designer kapısında üretilir, Build kapısı boyunca güncellenir, QA kapısında observed metrics ile doldurulur.

```markdown
# Stage Plan — <Oyun Adı>

## 1. Özet
- Tür: <match-3 / merge / hyper-casual / idle / puzzle / adventure>
- Launch stage sayısı: <N>
- World / biome sayısı: <M>
- Avg session hedefi: <60–180s>
- Milestone cadence: her <X> stage'de bir boss/milestone

## 2. World / Biome Listesi
| # | World | Stage Aralığı | Palette | Dominant Prop | BG Music | Narrative Beat |
|---|---|---|---|---|---|---|
| 1 | <Sunlit Meadow> | 1–25 | warm-yellow | flower | meadow-loop-01 | giriş, karakter tanışması |
| 2 | ... | ... | ... | ... | ... | ... |

## 3. Difficulty Curve
- Normal level first-try hedef: 55–75%
- Hard level: her 8–12 level'da bir
- Boss / milestone: her 25 stage'de bir
- Rest level: her 4–5 level'da bir

## 4. Tutorial Dağılımı
| Stage | Ne öğretiyor | Tutorial tipi |
|---|---|---|
| 1 | core mechanic | intuitive (no popup) |
| 4 | objective variant | guided overlay |
| 9 | revive / rewarded ad | popup + skip |
| 16 | yeni mekanik A | simple isolated stage |

## 5. Stage Matrisi
(launch N stage — her satır bir stage; buradaki tablo özettir, detay aşağıda)

| ID | World | Tier | Variant | Objective | New Mech | Reward | Intro | Outro |
|---|---|---|---|---|---|---|---|---|
| stg-001 | 1 | easy | standard | clear 20 tile | — | coin+10 | none | tip |
| stg-005 | 1 | normal | standard | score 5000 | combo-bonus | coin+25 | dialog | chest |
| stg-025 | 1 | boss | boss | clear board in 15 moves | — | chest+world-unlock | cinematic | cam-pan |
| ... | | | | | | | | |

## 6. Stage Detay Şeması (her stage için)

### stg-<id>
- **world**: <world_id>
- **tier**: easy | normal | hard | super-hard | boss
- **variant**: standard | puzzle-twist | rest | bonus | boss
- **theme**: <tek cümle>
- **visual_bg**: <assets/bg/<slug>>
- **palette**: <warm / cool / neutral>
- **props**: [<list>]
- **intro**: none | cinematic:<clip> | dialog:<text-id> | tip:<text-id>
- **objective**: <clear / score / collect / survive>
- **target_score_or_goal**: <number>
- **time_limit_sec**: <opt>
- **moves_limit**: <opt>
- **new_mechanic**: <slug or null>
- **reward**: { coin, booster, xp, chest_tier, unlock }
- **outro**: reward-popup | animation:<clip> | story-beat:<id> | cam-pan
- **unlock_next**: instant | star-gate:<N> | timer:<sec> | cinematic-wait
- **audio**: bg_music, ambient, boss_remix
- **first_try_target_%**: <int>
- **observed_completion_rate**: <doldurulur QA kapısında>
- **observed_attempts_avg**: <doldurulur>
- **churn_%**: <doldurulur>
- **fun_rating**: <1–5, doldurulur>
- **action**: keep | rebalance | cut

## 7. Progression UI
- Patern: <linear-path | hub | chapter-card | grid | diorama>
- Map zoom: <yes/no>
- Star-gate kullanılıyor mu: <yes/no, kaç yıldız>

## 8. Live-ops planı
- Patch cadence: 2 haftada 1
- Drop başına level sayısı: 10–20
- Event stage sıklığı: ayda 1
- Event stage tipi: milestone / seasonal / collab

## 9. Risk ve Cut-list
- Cut adayı stage'ler (playtest sonrası completion < %10): <liste>
- Rebalance kuyruğu: <liste>
```

---

## Özet (sahip için)

**Level count tavsiyesi (launch)**:
- Match-3 / merge live-ops oyunları: **100–120 stage + 3–5 world** (altında retention çöker).
- Hyper-casual / tek mekanik: **30–50 stage**.
- Idle: **10–15 world + 1 prestige layer**.
- Premium puzzle: **15–25 level**.
- Live-ops kadansı bizim ölçeğimizde: 2 haftada 10–20 level + aylık 1 event stage.

**Stage-plan.md zorunlu alanları** (her oyunda doldurulmadan Build kapısı açılmaz): `stage_id`, `world`, `tier`, `variant`, `objective`, `new_mechanic`, `reward`, `intro/outro affordance`, `unlock_next`, `target first-try %`, `visual palette`, `audio cue`. QA kapısında `observed_completion_rate`, `churn_%`, `action` alanları dolar.

**Yeni agent rolü**: Evet, **Level Designer** ayrı ajan olmalı (sonnet). Game Designer **core loop + monetization noktaları + difficulty ilkeleri** üretir; Level Designer **N adet stage'i brief'ler, stage-plan.md'yi yazar, JSON level dosyalarını üretir, QA data ile rebalance turu yapar**. Playrix/Dream bu ikiliyi ayırıyor; küçük oyunda bile tek kişiye yüklemek kapıyı tıkıyor. Alternatif: Game Designer'ın içinde "level-authoring" skill'i (`level-authoring.md`) tutup aynı agent'a bırakmak — MVP için kabul edilebilir, ship ölçeği büyürse ayrıştır.

**Sistem kuralı**: Yeni dosya `.claude/rules/level-design.md` açılmalı (stage planning disiplini, difficulty hedefleri, tutorial dağılımı kuralları, cut-list zorunluluğu). CLAUDE.md "Kurallar indeksi" bölümüne eklenmeli. Ayrıca `.claude/workflows/new-game-lifecycle.md` Design kapısı çıktısına `stage-plan.md` zorunlu artefakt olarak eklenmeli ve Build kapısı `stage-plan.md` yokken açılmamalı. Template iskelet'i `templates/stage-plan.md` altına konur, Game/Level Designer bu iskeletten başlar.

## Kaynaklar
- [Royal Match live-ops (Dream Games help)](https://dreamgames.helpshift.com/hc/en/3-royal-match/section/15-new-levels-1608112030/)
- [Royal Match revenue & stats (Business of Apps)](https://www.businessofapps.com/data/royal-match-statistics/)
- [How King defines a 'good' Candy Crush level (MobileGamer.biz)](https://mobilegamer.biz/how-king-defines-a-good-candy-crush-saga-level-and-why-it-constantly-prunes-the-bad-ones/)
- [Difficulty (Candy Crush wiki)](https://candycrush.fandom.com/wiki/Difficulty)
- [Level balancing in Candy Crush (robguilar.com)](https://www.robguilar.com/posts/candy_crush_difficulty/)
- [Match-3 metrics (GameAnalytics)](https://www.gameanalytics.com/blog/match-3-games-metrics-guide)
- [Match-3 scenarios (devtodev)](https://docs.devtodev.com/scenarios-and-best-practices/match-3)
- [Room 8 Studio: match-3 level design state](https://room8studio.com/news/smart-casual-the-state-of-tile-puzzle-games-level-design-part-1/)
- [CrazyLabs: hyper-casual level design](https://www.crazylabs.com/blog/the-must-have-of-level-design-in-hyper-casual-mobile-games/)
- [Supersonic: hyper-casual level design tips](https://supersonic.com/learn/blog/3-tips-for-improving-the-level-design-of-your-hyper-casual-game)
- [Sunday.gg: mobile level design for ad-monetized](https://sunday.gg/mobile-game-level-design-key-considerations-for-ad-monetized-casual-mobile-games/)
- [Level Design Book — Pacing](https://book.leveldesignbook.com/process/preproduction/pacing)
- [Game Developer: difficulty curves](https://www.gamedeveloper.com/design/difficulty-curves-how-to-get-the-right-balance-)
- [Game Developer: flow channel](https://www.gamedeveloper.com/design/game-design-theory-applied-the-flow-channel)
- [Game Developer: introducing mechanics](https://www.gamedeveloper.com/design/game-design-introducing-mechanics)
- [Game Developer: player progress in puzzle games](https://www.gamedeveloper.com/design/the-player-s-progress-designing-levels-for-mobile-puzzle-games)
- [Kongregate: math of idle games](https://blog.kongregate.com/the-math-of-idle-games-part-iii/)
- [Merge Dragons levels](https://mergedragons.fandom.com/wiki/Levels)
- [Game UI Database — Level select](https://www.gameuidatabase.com/index.php?scrn=42)
- [Lancaric: mobile game churn rate](https://lancaric.me/churn-rate-mobile-games/)
- [GameAnalytics: 50+ KPIs](https://www.gameanalytics.com/blog/50-kpi-measure-mobile-game-app)
