# Log Kuralları

İki ayrı log katmanı var; **karıştırma**.

## 1. Orkestrasyon log'u (factory MCP → logs tablosu)
Amaç: Agent organizasyonunun kapı kararlarını izlenebilir yapmak.

- **Batch zorunlu**: her agent kapı kapanışında **tek** `log_append` çağırır.
- Alanlar:
  - `agent` (zorunlu)
  - `decision` — ≤160 karakter, tek satır, karar özeti
  - `why` — kısa gerekçe (≤240 char)
  - `gameId`, `gate` — varsa
- **Chatter yasak**: mesaj başına log yok, subagent içi düşünce logu yok.
- `log_tail(limit=20)` → bir bakışta süreç.

### İyi örnek
```
agent: game-designer
decision: Core loop: 1-tap flap, 15s round, combo=streak-based
why: market.md'deki 3 rakipten farklı: kısa süre + kombo basıncı
```

### Kötü örnek
```
"Tasarıma başladım" ← karar değil, chatter
"Belki böyle olabilir" ← karar değil, müzakere
```

## 2. Uygulama log'u (oyun runtime)
Amaç: ship edilmiş oyunda bug teşhisi.

- `Microsoft.Extensions.Logging.ILogger<T>` inject.
- Seviye: `Trace/Debug` dev-only; `Information/Warning/Error` Release.
- Format: structured logging (`logger.LogError(ex, "Save failed for {GameState}", state)`).
- PII yasak: kullanıcı adı, email, konum → log'a girmez.
- Release'de local file + (opsiyonel) user-submit-bug akışına bağlı; telemetry backend yok (local-only kural).

## Çapraz kural
Ship'ten önce:
- Uygulama log'unda `Debug.WriteLine` bırakma.
- MCP log'unda son kapı için `log_append` atıldı mı, kontrol et (`log_tail(gameId=...)`).

## Retention
- MCP `logs` tablosu: 6 aydan eski satırlar Infrastructure agent tarafından özetlenir ve silinir (vacuum).
- Uygulama runtime log'u: cihazda 7 gün circular, 5 MB cap.
