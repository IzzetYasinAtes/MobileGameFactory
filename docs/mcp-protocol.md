# MCP Protokol — factory server

Bu doküman `tools/mcp-server` tarafından sunulan tüm MCP tool'ları, DB şemasını ve etkileşim desenlerini açıklar.

## Bağlantı
- Transport: **stdio** (Claude Code `.mcp.json` üzerinden otomatik spawn).
- Server adı: `factory`.
- Tool ad formatı (Claude tarafında): `mcp__factory__<tool_name>`.

## Tool listesi

### Oyun yaşam döngüsü
| Tool | Parametre | Döner | Açıklama |
|---|---|---|---|
| `game_create` | id, title, brief | `{ok, id, gate}` | Yeni oyun; kapı `intake`. |
| `game_list` | gate? | dizi `{id,title,gate,updatedAt}` | Tüm oyunlar veya gate filtreli. |
| `game_get` | id | `{id,title,gate,brief,meta,createdAt,updatedAt}` | Tek oyun. |
| `game_meta_patch` | gameId, patchJson | `{ok, meta}` | meta_json shallow merge. |
| `gate_advance` | gameId, nextGate | `{ok, id, gate}` | Bir sonraki kapıya geç (sıralı zorunlu). |

### Mesajlaşma (agent-to-agent)
| Tool | Parametre | Döner | Açıklama |
|---|---|---|---|
| `message_send` | from, to, type, subject, body, gameId? | `{ok, id}` | Kısa mesaj (body ≤400 char). |
| `inbox_pop` | agent, limit? | dizi | Okunmamışları al + read_at damgala. |
| `inbox_peek` | agent, limit? | dizi | Görüntüle (read işaretleme yok). |

Mesaj tipleri: `info | handoff | question | escalation | decision`.

### Log
| Tool | Parametre | Döner | Açıklama |
|---|---|---|---|
| `log_append` | agent, decision, why?, gameId?, gate? | `{ok, id}` | **Kapı-seviyesi** karar. Agent başına kapı başına 1 cağrı. |
| `log_tail` | limit?, gameId?, agent? | dizi | Son N log. |

### Artifact
| Tool | Parametre | Döner | Açıklama |
|---|---|---|---|
| `artifact_register` | gameId, gate, kind, path, note? | `{ok}` | Üretilen dosyayı oyuna bağla (içerik DB'ye girmez). |
| `artifact_list` | gameId | dizi | Oyunun tüm artefaktları. |

`kind` değerleri: `brief | market | design | code | qa | monetization | release | asset | notes`.

## Mesaj şeması (message_send body)
Body'de kısa, eyleme yönelik metin olmalı:
```
subject: "design.md hazır"
body: "Core: 1-tap flap, 15s tur. Risk: monetization noktası 2 çok agresif, denge için öneri var."
```

Body 400 char'ı aşıyorsa:
- Uzun kısmı dosyaya yaz (örn: `games/<id>/design.md`).
- `artifact_register` çağır.
- Mesaj body'de yalnızca 3 satır özet + dosya path'i ipucu.

## Kapı ilerletme sırası
```
intake -> research -> design -> build -> qa -> release -> shipped
```
`gate_advance` yalnızca sıradaki kapıya geçer; atlamaz. Atlama gerekirse `game_meta_patch` ile sebep kayıt + infrastructure agent'a eskalasyon.

## DB şeması (özet)
```sql
games(id PK, title, gate, brief, meta_json, created_at, updated_at)
messages(id PK, game_id, from_agent, to_agent, type, subject, body, created_at, read_at)
logs(id PK, game_id, agent, gate, decision, why, created_at)
artifacts(id PK, game_id, gate, kind, path, note, created_at, UNIQUE(game_id,kind,path))
```
İndeksler: `messages(to_agent, read_at, id)`, `logs(game_id, id)`, `artifacts(game_id)`.

Tam sema: `tools/mcp-server/Storage/FactoryDb.cs`.

## Etkileşim desenleri

### Desen 1: Agent handoff
```
A: artifact_register(...)
A: message_send(to="project-manager", type="handoff", ...)
A: log_append(gate=..., decision=..., why=...)
```
3 çağrıdan fazlasına ihtiyaç varsa tasarımı gözden geçir.

### Desen 2: Eskalasyon
```
Any: message_send(to="project-manager", type="escalation", body="<somut öneri>")
PM : inbox_pop → düşün → message_send(type="decision", to=<from>) → log_append
```

### Desen 3: Okuma yoğun query
`game_list` + `log_tail` + `artifact_list` — /status komutu bu üçünü birleştirir.

## Extensibility (yeni tool eklemek)
1. `tools/mcp-server/Tools/XYZTools.cs` oluştur.
2. `[McpServerToolType]` class, method'a `[McpServerTool(Name="...")]` + `[Description]`.
3. `FactoryDb.Read<T>` veya `FactoryDb.Write<T>` ile DB erişimi.
4. Bu dokümanda tablo güncelle.
5. Infrastructure agent bu değişikliği yapan taraftır (oyun agent'ları değil).

## Versiyonlama
Tool adı breaking change'te versiyon süffix'i: `log_append` → `log_append_v2`. Eski tool geriye uyumluluk için 1 sürüm daha yaşar, sonra kaldırılır.
