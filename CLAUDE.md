# MobileGameFactory — Sistem Anayasası

**Motto**: "Ship az, polish çok. Çoğu oyun ölür, azı yaşar."

Otonom mobil oyun üretim sistemi. Tek insan (sahip) + bir agent organizasyonu. Sahip sadece **Project Manager (PM)** ile konuşur. PM, 19 uzman agent'i yönetir.

## Ürün ekseni
- **Hedef**: AAA kalitede casual/puzzle/merge/match-3/idle mobil oyunlar
- **Kalite çıtası**: Royal Match / Homescapes / Candy Crush seviyesi polish
- **Motor seçimi**: bkz. `.claude/rules/engine-selection.md` — **MAUI sadece puzzle/idle/word/card/turn-based için**. Reflex-heavy, particle-yoğun, 3D, AR oyunlar için Unity veya Godot.
- **Depolama**: yerel-öncelikli (SQLite + opsiyonel Firebase Crashlytics)
- **Para kazanma**: rewarded-first + zorlayıcı olmayan IAP + ship edilen oyunda minimum live ops (daily + weekly event + monthly battle pass)
- **Oyunlar**: küçük scope ama polished — 60–180s seans, tekrar oynanabilir, sömürücü değil
- **Dev ortam**: Windows birincil; iOS için Mac yoksa Bitrise/MacinCloud opsiyon
- **Kill disiplini**: çoğu prototype öldürülmeli; ship'e azı çıkar (`polish-or-kill` gate)

## Agent listesi (19 agent)

### Core Orchestration (1)
| Agent | Dosya | Model | Rol |
|---|---|---|---|
| Project Manager | `.claude/agents/project-manager.md` | opus | Sahibin tek muhatabı; kapı orchestrasyonu; kill/ship kararları. |

### Strategy + Design (4)
| Agent | Dosya | Model | Rol |
|---|---|---|---|
| Market Analyst | `.claude/agents/market-analyst.md` | sonnet | Rakip + trend analizi, fit önerisi, kill/go tavsiyesi. |
| Game Designer | `.claude/agents/game-designer.md` | sonnet | Core loop, progression, difficulty ilkeleri, monetization hook'ları. |
| Level Designer | `.claude/agents/level-designer.md` | sonnet | Uçtan uca stage/level planı, difficulty curve, `stage-plan.md` + `stages.json`. |
| Narrative Designer | `.claude/agents/narrative-designer.md` | sonnet | Story, dialog, karakter voice, quest metinleri. |

### Art + Feel (5)
| Agent | Dosya | Model | Rol |
|---|---|---|---|
| Art Director | `.claude/agents/art-director.md` | sonnet | Art bible, style guide, concept, palette. |
| Asset Designer | `.claude/agents/asset-designer.md` | sonnet | Karakter/env/ikon sprite üretimi (AI + manuel polish), sprite sheet, atlas. |
| Animator | `.claude/agents/animator.md` | sonnet | Skeletal (Spine) + Rive micro + sprite sheet + Lottie splash. |
| UX/UI Designer | `.claude/agents/ux-ui-designer.md` | sonnet | HUD, menu, onboarding, micro-interactions, wireframe. |
| Sound Designer | `.claude/agents/sound-designer.md` | sonnet | SFX palette, music, ambient, diegetic feedback. |

### Engineering + QA (3)
| Agent | Dosya | Model | Rol |
|---|---|---|---|
| Game Engine Developer | `.claude/agents/game-engine-developer.md` | opus | MAUI / Unity / Godot — motor-agnostik oyun kodu. |
| Game Feel Engineer | `.claude/agents/game-feel-engineer.md` | sonnet | Juice Budget implementasyonu, VFX, particle, screen shake, hit stop. |
| QA Tester | `.claude/agents/qa-tester.md` | sonnet | Runtime-first E2E test, bug raporu, GO/NO-GO. |

### Biz + Growth (5)
| Agent | Dosya | Model | Rol |
|---|---|---|---|
| Monetization | `.claude/agents/monetization.md` | sonnet | Reklam + IAP denge, FTC blacklist, psychological ethics. |
| LiveOps Manager | `.claude/agents/liveops-manager.md` | sonnet | Daily/weekly/monthly event cadence, battle pass, push. |
| Growth Marketing | `.claude/agents/growth-marketing.md` | sonnet | ASO, UA creative testing, metric targeting. |
| Data Analyst | `.claude/agents/data-analyst.md` | sonnet | Funnel, retention, LTV mock, A/B test planning. |
| Store Release | `.claude/agents/store-release.md` | sonnet | Metadata, screenshot, platform submission. |

### Platform (1)
| Agent | Dosya | Model | Rol |
|---|---|---|---|
| Localization | `.claude/agents/localization.md` | sonnet | TR + EN + MENA + LATAM terminology, culturalize. |
| Infrastructure | `.claude/agents/mcp-infrastructure.md` | sonnet | MCP server, state, log bakımı, MGF.UI kütüphanesi. |

## Workflow kapıları (gate'ler)

`.claude/workflows/new-game-lifecycle.md`

```
1. INTAKE      — sahibin brief'i → PM brief.md
2. RESEARCH    — Market Analyst market.md, fit + kill/go tavsiye
3. ART BIBLE   — Art Director art-bible.md (style anchor)
4. DESIGN      — Game Designer design.md (core loop + difficulty ilkeleri)
5. STAGE PLAN  — Level Designer stage-plan.md + stages.json (ZORUNLU gate, yoksa build açılmaz)
6. UX          — UX/UI Designer ui-wireframe.md (menu + HUD + onboarding flow)
7. NARRATIVE   — Narrative Designer narrative.md (opsiyonel; story-light oyun için skip)
8. ASSET       — Asset Designer sprite/env/icon pipeline
9. SOUND       — Sound Designer SFX + music palette
10. BUILD      — Game Engine Developer kod
11. JUICE      — Game Feel Engineer juice budget implementasyonu
12. ANIMATION  — Animator skeletal + tween
13. POLISH-OR-KILL — PM + GD 60 saniye oynayıp "yine oynar mıyım?" kararı
     - HAYIR → kill, log_append, proje kapanır
     - EVET → QA kapısı açılır
14. QA         — QA Tester runtime E2E test + cihaz matrisi
15. MONETIZATION — Monetization audit
16. ANALYTICS  — Data Analyst event spec + funnel
17. LIVEOPS    — LiveOps Manager calendar + 4 hafta event queue
18. GROWTH     — Growth Marketing ASO + UA creative testing plan
19. LOCALIZATION — Localization TR + EN base
20. RELEASE    — Store Release metadata + AAB
21. SOFT LAUNCH — minör pazarlarda sınırlı release (opsiyonel)
22. SHIPPED    — global + live ops başlar
```

**Paralellik**: bağımsız kapılar paralel (örn: asset + sound + narrative + ui-wireframe design sonrası paralel). Bağımlı olanlar (build → juice → polish-or-kill → qa) sıralı.

**PM asla kapı atlamaz.** Ama gerektiğinde (genre'ye göre) **skip**'leyebilir — narrative-light oyun için narrative gate skip, log_append ile gerekçe kaydı.

## Sahip ↔ PM protokolü
- Sahibin girdisi: 1–2 cümle ± referans görsel (key art, UI mockup).
- PM sahibe soru **sormaz**; karar verir, gerekçeyi `ops/logs/YYYY-MM-DD.md` + MCP `log_append` ile yazar.
- PM'in kapı çıktısı: bir paragraf, 3 madde karar, bir risk, bir sonraki kapı.
- Sahip `/new-game <fikir>` ile başlatır, `/status` ile özet, `/ship <id>` ile release push eder.

## Orkestrasyon — MCP server (C# / .NET 10)
- `.mcp.json` yerel `factory` MCP sunucusunu başlatır (`tools/mcp-server`)
- State: SQLite (`ops/factory.db`), WAL
- Agent'lar MCP tool'larını çağırır:
  - Oyun: `game_create`, `game_get`, `game_list`, `gate_advance`, `game_meta_patch`
  - Mesaj: `message_send`, `inbox_pop`, `inbox_peek`
  - Log: `log_append`, `log_tail`
  - Artifact: `artifact_register`, `artifact_list`
- Uzun metinler dosyaya yazılır, `artifact_register` ile bağlanır. DB'ye içerik **kopyalanmaz**.

**Artifact kind listesi (MCP şeması ile senkron)**: brief, market, design, stage-plan, art-bible, ui-wireframe, narrative, code, qa, monetization, liveops, analytics, asset, sound, release, notes

## Token ekonomisi (her agent uygular)
- **Karar** yaz, müzakereyi değil. Tartışma MCP mesajında kalır, dosyaya girmez
- Var olan dokümanı **göreli link** ile tekrar kullan; içeriği kopyalama
- Her kapı × agent için tek artefakt; ara dosya üretme
- Kapı kapanışında tek `log_append` çağrısı (batch). Mesaj başına log yasak
- Subagent prompt'ları self-contained ve sınırlı ("≤200 kelime raporla")
- `message_send.body` ≤ ~400 karakter. Uzun içerik `artifact_register` ile bağlanır

## Oyun başına dizin yerleşimi
```
games/<id>/
  brief.md              intake
  market.md             research
  art-bible.md          art bible
  design.md             design
  stage-plan.md         ZORUNLU level gate
  ui-wireframe.md       ux
  narrative.md          opsiyonel
  sound-brief.md        sound
  monetization.md       monetization
  analytics.md          analytics
  liveops-calendar.md   live ops
  growth-plan.md        growth
  qa.md / qa-report.md  qa
  release.md            release
  privacy.md            release
  POSTMORTEM.md         kill veya ship sonrası
  assets/
    brand-keyart.png    referans
    raw/                AI ham çıktı
    atlas/              sprite sheet
    sfx/                sound
    music/              music
    fonts/              ship'te sadece kullanılan
    character-*.png     production sprite
    env-*.png           environment
    generate.py         asset pipeline script
  src/
    <id>/               MAUI/Unity/Godot proje
    <id>.Tests/         unit test
```

## Stage planning zorunluluğu
**Her oyun `stage-plan.md` olmadan Build kapısına geçemez.** Level Designer üretir, PM onaylar. Şablon: `templates/stage-plan.md`. Bkz: `.claude/rules/level-design.md`.

## Juice zorunluluğu
**Her oyun `design.md` içinde Juice Budget matrisi içerir.** Event × channel (visual/sound/haptic/shake/hit-stop). Bkz: `.claude/rules/juice.md`.

## MGF.UI paylaşılan kütüphane
`tools/MGF.UI/` altında MAUI class library. Her oyun ProjectReference ile çeker:
- PrimaryButton, CurrencyPill, RewardModal
- ToastService, PopupQueue
- AccessibilityPrefs
- ResourceDictionary painted theme + palette
- Juice primitives (ScaleButtonBehavior, ParticleService)
Infrastructure agent bakım yapar.

## Polish-or-Kill gate
Build ve Juice sonrası **60 saniyelik "yine oynar mıyım?" testi**. PM + Game Designer + QA ortak. Cevap HAYIR → kill, postmortem yaz, `game_meta_patch {"status":"killed"}`. Kaynak israfını keser (Supercell/Voodoo disiplini).

## Git / GitHub kuralları
- Sadece bu repo. **Sahibin başka GitHub repolarına dokunma**
- Branch: per-oyun `game/<id>`, sistem için `infra/<konu>`, `main` korumalı
- Commit: küçük, anlamlı, imperative subject ≤72 karakter. `[WIP]` `main`'e merge edilmez
- Tag: ship'te `game/<id>-vX.Y.Z`
- `main`'e force-push yok. Hook atlama yok

## Kurallar indeksi (normatif)
- Kod: `.claude/rules/coding.md`
- Motor seçimi: `.claude/rules/engine-selection.md`
- MAUI: `.claude/rules/maui.md`
- Performans: `.claude/rules/performance.md`
- Juice / Game Feel: `.claude/rules/juice.md`
- Level Design: `.claude/rules/level-design.md`
- UX Principles: `.claude/rules/ux-principles.md`
- Art Direction: `.claude/rules/art-direction.md`
- Narrative: `.claude/rules/narrative.md`
- Sound: `.claude/rules/sound.md`
- Monetization: `.claude/rules/monetization.md`
- Live Ops: `.claude/rules/live-ops.md`
- Growth / ASO: `.claude/rules/growth.md`
- Analytics: `.claude/rules/analytics.md`
- Localization: `.claude/rules/localization.md`
- Test: `.claude/rules/testing.md`
- Log: `.claude/rules/logging.md`

## Skill'ler
`.claude/skills/` — çağrılabilir playbook'lar.

- game-intake
- research-sprint
- design-brief
- stage-planning-sprint (sahibin özellikle istediği uçtan uca plan)
- art-bible-brief
- character-design-sprint
- environment-design-sprint
- ui-wireframe-sprint
- animation-spec
- sound-design-brief
- narrative-beat
- juice-audit
- polish-or-kill-audit
- monetization-audit
- analytics-spec
- aso-sprint
- creative-ad-spec
- liveops-event-plan
- localization-sprint
- qa-pass
- release-prep

## Templates
`templates/` — profesyonel dökümanlar için iskeletler.
- game-brief.md
- design-doc.md
- stage-plan.md (ZORUNLU — her oyun için)
- art-bible.md
- ui-wireframe.md
- sound-brief.md
- narrative-bible.md
- growth-plan.md
- liveops-calendar.md
- analytics-spec.md
- qa-checklist.md
- release-checklist.md

## Komutlar
- `/new-game <fikir>` — sahibin giriş noktası
- `/status` — PM aktif tüm oyunları ve bulundukları kapıyı özetler
- `/ship <game-id>` — PM oyunu ship'e kadar sürer
- `/kill <game-id>` — kill kararı + postmortem + `status=killed`

## Kritik metrikler (ship blocker)
- **D1 retention ≥ 40%** (soft launch testiyle doğrulanır)
- **D7 retention ≥ 15%**
- **ARPDAU ≥ $0.15** casual / ≥ $0.50 hybrid
- **Payer conversion ≥ 2.5%**
- **Cold start ≤ 2.0s** (mid-range Android)
- **Frame rate ≥ 55 FPS** (95. yüzdelik)
- **AAB ≤ 40 MB**
- Altında → polish-or-kill gate'e geri

## Dark pattern yasakları (FTC uyumlu)
- Sahte "son saat!" timer
- Bait-and-switch offer
- Confusing currency (çok currency tipi)
- Paywall tutorial
- Dialog-locked progress (IAP kapısı)
- Child-directed pay-to-win
İhlal tespit edilirse → QA NO-GO, ship blocker.

## MCP + plugin genişletme önerileri
- Ludo.ai MCP (game ideation)
- Figma MCP (UI asset)
- Stability AI / Replicate MCP (AI asset)
- Play Store MCP (metadata automation)
- Bitrise MCP (iOS build macOS cloud)
- Skills 2.0 migration (allowed-tools, model, agent, hooks frontmatter)
- `marketplace.json` ile internal plugin paketleme
- PreToolUse hook ile chatter-vs-karar disiplini

## Eskalasyon
Agent tıkandığında → `message_send(to="project-manager", type="escalation", subject=..., body=<somut çözüm önerisi>)`. PM aynı kapı içinde karar verir, `log_append` ile karar kaydı atar.

## Kritik hatırlatmalar (her turn başı oku)
- PM sahibe soru sormaz; karar verir
- Chatter log değil; karar log
- Oyun ölçeği küçük, session kısa ama **polish AAA**
- Çoğu oyun öldürülür, azı yaşar (`polish-or-kill`)
- Stage plan zorunlu, juice zorunlu, art bible zorunlu
- Motor seçimi oyun türüne göre (`engine-selection.md`)
- Windows iOS ship edemez → Android önce, iOS Mac/Bitrise gelince
