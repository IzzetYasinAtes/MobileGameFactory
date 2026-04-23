# MobileGameFactory — Sistem Anayasası

Otonom mobil oyun üretim sistemi. Tek insan (sahip) + bir agent organizasyonu. Sahip sadece **Project Manager (PM)** ile konuşur. PM diğer 7 agent'i yönetir.

## Ürün ekseni (pazarlık konusu değil)
- Teknoloji: **.NET 10 + .NET MAUI**, Android + iOS hedefi.
- Depolama: **yalnız yerel** (SQLite — `sqlite-net-pcl`). Backend yok, cloud yok, sync server yok.
- Para kazanma: **rewarded ad** (opt-in, değer karşılığı) + **zorlayıcı olmayan IAP**. Interstitial nadir ve session-gated. Bkz. `.claude/rules/monetization.md`.
- Oyunlar: küçük, polished, kısa seanslı (60–180s). Tekrar oynanabilir, sömürücü değil.
- Geliştirme ortamı: Windows. iOS yayın için macOS gerekli — bkz. `docs/store/windows-dev-reality.md`.

## Agent listesi
| Agent | Dosya | Model | Rol |
|---|---|---|---|
| Project Manager | `.claude/agents/project-manager.md` | opus | Tek kullanıcı temas noktası. Karar verir, sıralar, ship eder. |
| Market Analyst | `.claude/agents/market-analyst.md` | sonnet | Rakip + trend analizi, fit önerisi. |
| Game Designer | `.claude/agents/game-designer.md` | sonnet | Core loop, progression, difficulty, monetization noktaları. |
| MAUI Developer | `.claude/agents/maui-developer.md` | opus | Oyunu `/src/<game-id>/` altında kodlar. |
| QA Tester | `.claude/agents/qa-tester.md` | sonnet | Test matrisi, edge case, bug listesi. |
| Monetization | `.claude/agents/monetization.md` | sonnet | Reklam/IAP yerleşim dengesi. |
| Store/Release | `.claude/agents/store-release.md` | sonnet | Metadata, ASO, platform submission. |
| Infrastructure | `.claude/agents/mcp-infrastructure.md` | sonnet | `/ops/` protokolü, log, state bakımı. |

## Workflow kapıları (gate'ler) — bkz. `.claude/workflows/new-game-lifecycle.md`
1. **Intake** — sahibin brief'i → PM `ops/state/<id>.json` + `docs/games/<id>/brief.md` yazar.
2. **Research** — Market Analyst `market.md` üretir; PM kapı onayı.
3. **Design** — Game Designer `design.md` üretir; PM kapı onayı.
4. **Build** — MAUI Developer `src/<id>/` üretir; Monetization + Infrastructure girdi verir.
5. **QA** — QA Tester `qa.md` üretir, bug'lar kapatılır.
6. **Release hazırlık** — Store/Release `release.md` + asset checklist üretir.
7. **Ship** — PM imzalar, `game/<id>-v1.0.0` tag'ler, sahibe tek paragraf özet yazar.

PM asla kapı atlamaz. PM bağımlı olmayan kapıları paralel çalıştırabilir.

## Sahip ↔ PM protokolü
- Sahibin girdisi: 1–2 cümle ± referans görsel.
- PM sahibe soru **sormaz**. Karar verir, gerekçeyi `ops/logs/YYYY-MM-DD.md` içine "Neden:" satırıyla loglar.
- PM'in kapı çıktısı: bir paragraf, 3 madde karar, bir risk, bir sonraki kapı.

## Orkestrasyon — MCP server (C# / .NET 10)
- `.mcp.json` yerel `factory` MCP sunucusunu başlatır (`tools/mcp-server`). Detay: `docs/mcp-protocol.md`.
- State, mesaj, log, artifact kayıtları **SQLite** (`ops/factory.db`). Dosya tabanlı inbox yok.
- Agent'lar dosya yazmak yerine MCP tool'larını çağırır (kısa, yapılandırılmış, token dostu):
  - Oyun: `game_create`, `game_get`, `game_list`, `gate_advance`, `game_meta_patch`
  - Mesaj: `message_send`, `inbox_pop`, `inbox_peek`
  - Log (kapı-seviyesi karar): `log_append`, `log_tail`
  - Artifact (üretilen dosya referansı): `artifact_register`, `artifact_list`
- Uzun metinler (design doc, market raporu, brief) dosyaya yazılır; konumu `artifact_register` ile oyuna bağlanır. DB'ye içerik **kopyalanmaz**.

## Token ekonomisi kuralları (her agent uygular)
- **Karar** yaz, müzakereyi değil. Tartışma MCP mesajında kalır, dosyaya girmez.
- Var olan dokümanı **göreli link** ile tekrar kullan; içeriği kopyalama.
- Her kapı × agent için tek artefakt; ara dosya üretme.
- Kapı kapanışında tek `log_append` çağrısı (batch). Mesaj başına log **yasak**.
- Subagent prompt'ları self-contained ve sınırlı olmalı ("≤200 kelime raporla").
- MCP `message_send.body` ≤ ~400 karakter. Uzun içerik `artifact_register` ile bağlanır.

## Git / GitHub kuralları
- Sadece bu repo. **Sahibin GitHub hesabındaki başka repolara asla dokunma.**
- Branch: per-oyun `game/<id>`, sistem için `infra/<konu>`, `main` korumalı.
- Commit: küçük, anlamlı, imperative subject ≤72 karakter. `[WIP]` `main`'e merge edilmez.
- Tag: ship'te `game/<id>-vX.Y.Z`.
- `main`'e force-push yok. Hook atlama yok.

## Kurallar indeksi (normatif)
- Kod: `.claude/rules/coding.md`
- MAUI: `.claude/rules/maui.md`
- Performans: `.claude/rules/performance.md`
- Monetization: `.claude/rules/monetization.md`
- Test: `.claude/rules/testing.md`
- Log: `.claude/rules/logging.md`

## Skill'ler
`.claude/skills/` — çağrılabilir playbook'lar: `game-intake`, `research-sprint`, `design-brief`, `monetization-audit`, `qa-pass`, `release-prep`.

## Komutlar
- `/new-game <fikir>` — sahibin giriş noktası; PM'e devreder.
- `/status` — PM aktif tüm oyunları ve bulundukları kapıyı özetler.
- `/ship <game-id>` — PM oyunu ship'e kadar sürer.

## Oyun başına dizin yerleşimi
```
docs/games/<id>/
  brief.md        design.md       market.md
  qa.md           monetization.md release.md
src/<id>/         MAUI solution
```
Oyun state'i ve kapı durumu `ops/factory.db` içinde; dosyaya JSON state yazılmaz. Üretilen her dosya `artifact_register` ile oyuna bağlanır.

## Eskalasyon
Agent tıkandığında → `message_send(to="project-manager", type="escalation", subject=..., body=<somut çözüm önerisi>)`. PM aynı kapı içinde karar verir, `log_append` ile karar kaydı atar.

## Kritik hatırlatmalar (her turn başı oku)
- PM sahibe soru sormaz; karar verir.
- Chatter log değil; karar log.
- Oyun ölçeği küçük, session kısa.
- Windows iOS ship edemez → Android önce, Mac bulunduğunda iOS.
