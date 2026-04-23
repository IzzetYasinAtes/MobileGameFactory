# MobileGameFactory

Tek kişilik mobil oyun üretim hattı. Sen 1–2 cümle fikir verirsin; Project Manager agent araştırır, tasarlar, yaptırır, test ettirir, yayına hazırlar.

## Nasıl kullanılır

```
/new-game "Suçlu kuş, 15 saniyelik turlar, neon retro estetik"
```

PM devreye girer. Bundan sonra tek muhatabın PM. Diğer agent'leri PM görevlendirir.

## Proje iskeleti
- `CLAUDE.md` — sistem anayasası (ilk bu okunur)
- `.claude/agents/` — 8 agent (PM + 7 uzman)
- `.claude/rules/` — kod / MAUI / performans / monetization / test / log kuralları
- `.claude/skills/` — çağrılabilir playbook'lar
- `.claude/commands/` — `/new-game`, `/status`, `/ship`
- `.claude/workflows/new-game-lifecycle.md` — 7 kapı süreci
- `docs/` — mimari, mcp-protocol, araştırma, store rehberleri
- `ops/factory.db` — SQLite (agent mesajları + state + kararlar; runtime, gitignored)
- `tools/mcp-server/` — C# / .NET 10 MCP sunucusu (agent iletişimi ve state)
- `games/<game-id>/` — **her oyun tek klasörde**: docs, src (MAUI), assets
- `templates/` — oyun brief, design doc, QA checklist, release checklist
- `examples/sample-game-brief.md` — nasıl brief yazacağının örneği

## İlk kurulum (tek seferlik)
```bash
dotnet build MobileGameFactory.sln -c Release
```
Kök `.sln` her projeyi derler. Claude Code `.mcp.json` üzerinden MCP sunucusunu otomatik başlatır; build sonrası oturum yeniden açılırsa `factory` MCP server tool'ları hazır olur.

## Visual Studio ile açma
`MobileGameFactory.sln` dosyasını çift tıkla — VS Solution Explorer'da tüm klasörleri (`.claude`, `docs`, `templates`, `games`, `tools`) solution folder olarak göreceksin. Her yeni oyun `games/<id>/src/<id>/` altında kendi `.csproj`'u olarak eklenir ve otomatik aynı sln'de görünür.

## Her oyunun klasör yerleşimi
```
games/<id>/
  brief.md / design.md / market.md / monetization.md / qa.md / release.md
  assets/      (raw kaynaklar: icon PSD, ses kaynağı, vb.)
  src/
    <id>/      (MAUI ana projesi — kod, Resources, Platforms)
    <id>.Tests/ (xUnit)
```
Her oyunla ilgili **her şey** bu klasörde. Ship sonrası `privacy.md` da eklenir.

## Windows gerçeği (kısa)
- **Android:** Windows'ta tümüyle build + test (emulator + fiziksel cihaz).
- **iOS build + submit:** macOS zorunlu (Xcode + App Store Connect). Windows'tan remote Mac ya da cloud Mac olmadan iOS ship mümkün değil.
- PM bu durumu her oyunun release kapısında sana bildirir ve "Android-only ship" ya da "Mac gerekli" kararını netleştirir.

Detay: `docs/store/windows-dev-reality.md`.

## İlk oyunu nasıl başlatırım
1. `/new-game "<fikir>"` yaz — istersen referans görsel yapıştır.
2. PM brief'i `games/<id>/brief.md` olarak kaydeder, `ops/state/<id>.json` açar.
3. Kapılar ilerledikçe PM sana tek paragraflık özet verir; sen onaylar veya yön değiştirirsin.
4. Ship kapısında PM `game/<id>-v1.0.0` tag'ini atar ve store submission checklist'ini sunar.

## Kurallar (özet)
- PM sana soru sormaz, karar verir.
- Agent'lar arası gereksiz konuşma yok; sadece kapı-seviyesi karar loglanır.
- Her oyun küçük, hızlı, rewarded-ad dostu, IAP opsiyonel.
- Backend, cloud, external DB yok. Sadece yerel SQLite.

Sistemi tam anlamak için `CLAUDE.md` dosyasını oku.
