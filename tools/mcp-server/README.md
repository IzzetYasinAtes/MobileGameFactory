# MobileGameFactory MCP Server

Agent'lar arasi iletisimi, oyun state'ini, kapi (gate) kararlarini ve ciktilari **SQLite** uzerinde tutan C# (.NET 10) MCP sunucusu.

Dosya tabanli inbox/state yok — tum iletisim yapisal MCP tool cagrilariyla yapilir, boylece Claude baglami kucuk ve tahmin edilebilir kalir (**token tasarrufu**).

## Neden MCP + SQLite
- Mesajlar dosya yerine satir: agent inbox'unu tek cagriyla pop eder.
- Uzun metin DB'ye yazilmaz; dosya yolu `artifact_register` ile baglanir.
- Kapi ilerleyisi `gate_advance` ile kontrollu.
- Butun state `ops/factory.db` icinde — audit, backup, incelemesi kolay.

## Calistirma
Claude Code `.mcp.json` uzerinden otomatik baslatir. Manuel test:

```bash
dotnet run --project tools/mcp-server
```

`ops/factory.db` otomatik olusur (yoksa). `MGF_DB` env degiskeni ile yol override edilebilir.

## Tool ozeti

| Tool | Amac |
|---|---|
| `game_create` | Yeni oyun (intake kapisi) |
| `game_list` | Tum oyunlar / gate filtresi |
| `game_get` | Tek oyun kaydi |
| `game_meta_patch` | meta_json shallow merge |
| `gate_advance` | Kapiyi bir sonrakine ilerlet (sirali zorunlu) |
| `message_send` | Agent -> agent kisa mesaj |
| `inbox_pop` | Okunmamis mesajlari al + okundu isaretle |
| `inbox_peek` | Inbox'i read_at'siz goster |
| `log_append` | Kapi-seviyesi karar kaydi (batch) |
| `log_tail` | Son N kararlari getir |
| `artifact_register` | Uretilen dosyayi oyuna bagla |
| `artifact_list` | Oyunun tum ciktilari |

Detay: `docs/mcp-protocol.md`.

## DB semasi
Ayrintisi `Storage/FactoryDb.cs` icinde. 4 tablo: `games`, `messages`, `logs`, `artifacts`. WAL mode ve basit lock ile tek yazici.

## Versiyonlama
- `ModelContextProtocol` paketi preview'de; surum uyumsuzluklarinda `MobileGameFactory.Mcp.csproj` icindeki tek satiri guncelleyin.
- DB sema degisikligi gerekirse `Storage/FactoryDb.cs` `InitSchema` metodunu idempotent tutun (`CREATE TABLE IF NOT EXISTS`, `ALTER` ile geriye uyumlu ekleme).

## Sinirlar
- Tek client (stdio) icin tasarlandi. Cok-client gerekirse TCP/HTTP transport eklenir.
- DB'yi gitignore edin (runtime state). Sema koddadir.
