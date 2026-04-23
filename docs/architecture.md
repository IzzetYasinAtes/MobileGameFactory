# Mimari

## Yüksek seviye
```
         +--------------------+
Sahip -- | Claude Code (CLI)  |  /new-game, /status, /ship
         |  slash komutlar    |
         +--------+-----------+
                  |
                  v
         +--------------------+      Task tool
         |  PM agent (opus)   | ----------------------> diğer 7 agent (subagent)
         +--------+-----------+
                  |
       JSON-RPC / stdio (.mcp.json)
                  |
                  v
         +--------------------+
         |  factory MCP       |  C# / .NET 10 console
         |  (tools/mcp-server)|  ModelContextProtocol SDK
         +--------+-----------+
                  |
                  v
         +--------------------+
         |  ops/factory.db    |  SQLite (WAL)
         |  games, messages,  |
         |  logs, artifacts   |
         +--------------------+
```

## Aktörler
- **Claude Code CLI**: ana oturum; slash komutlardan skill/agent başlatır.
- **PM agent**: ana döngüyü yönetir, sahibe tek muhatap, subagent tetikler.
- **Uzman agent'lar** (7): kapı başına 1 uzman; PM tarafından Task tool ile çağrılır.
- **factory MCP server**: mesajlaşma, state, log, artifact depolama.
- **SQLite DB**: tüm kalıcı durum.

## Veri akışı (yeni oyun)
1. `/new-game "..."` → PM agent.
2. PM `game_create` + dosya (brief.md) + `artifact_register` + `gate_advance("research")` + `log_append`.
3. PM Task tool ile market-analyst subagent çağırır, prompt kısa.
4. market-analyst WebSearch/WebFetch kullanır, market.md yazar, `artifact_register` + `message_send(to="project-manager")`.
5. PM `inbox_pop` ile cevabı alır. Onaylarsa `gate_advance("design")`.
6. Zincir design → build → qa → release → shipped olarak devam.

## Neden bu tasarım
- **Token ekonomisi**: Agent'lar konuşma dosyaları oluşturmak yerine MCP tool çağrılarıyla yapılandırılmış JSON yollar. Claude context küçük kalır.
- **Kalıcılık**: DB tek kaynak. Session'lar arası süreklilik. Audit trail.
- **İzolasyon**: Her agent kendi uzmanlık alanında; chatter yok, kapı-seviyesi karar var.
- **Paralellik**: Bağımsız kapılar aynı anda koşabilir (monetization + qa).

## Kod haritası
- `tools/mcp-server/Program.cs` — MCP host başlatma.
- `tools/mcp-server/Storage/FactoryDb.cs` — SQLite encapsulation + WAL.
- `tools/mcp-server/Tools/*.cs` — 12 MCP tool, 4 dosyada.
- `.claude/agents/*.md` — 8 agent; subagent tanımları.
- `.claude/skills/*/SKILL.md` — 6 playbook.
- `.claude/commands/*.md` — 3 slash komut.
- `.claude/rules/*.md` — 6 normatif kural dosyası.
- `.claude/workflows/new-game-lifecycle.md` — 7 kapı şeması.

## Her oyunun dosya ayak izi
```
docs/games/<id>/
  brief.md       (intake)
  market.md      (research)
  design.md      (design)
  monetization.md (build/qa)
  qa.md          (qa)
  release.md     (release)
  privacy.md     (release; dış host'a yüklenir)
src/<id>/
  <id>.sln
  <id>/               MAUI shared project
  <id>.Tests/         xUnit
```

## State şeması (DB)
- **games**: id, title, gate, brief, meta_json, timestamps.
- **messages**: oyun × agent × mesaj (inbox/outbox ayrımı `to_agent`/`read_at`).
- **logs**: kapı-seviyesi kararlar (chatter değil).
- **artifacts**: üretilen dosyaların yol referansı (içerik yok).

## Sınırlar
- Tek MCP client (stdio). Çok-client için transport değişimi (TCP/HTTP) MCP-Infrastructure agent işi.
- DB 100 oyundan sonra büyüyebilir; retention Infrastructure agent'ın sorumluluğu.
- iOS build Windows'ta yapılamaz (platform sınırı).

## İleride
- Potansiyel genişlemeler:
  - Play Console MCP server (asset upload otomasyonu).
  - ASO keyword research MCP (3rd party API).
  - Screenshot automation MCP (Appium ya da MAUI emulator capture).
- Genişletme yolu: `.mcp.json` içine yeni server ekle; PM agent ilgili tool'ları öğrenir.
