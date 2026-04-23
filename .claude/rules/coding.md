# Kod Kuralları (C# / .NET 10)

## Stil
- File-scoped namespace (`namespace X;`).
- `var` tercih et; tür netliği gerektiren yerlerde açık tip.
- PascalCase public, camelCase local, `_camelCase` private field.
- Satır ≤ 120 karakter.
- `using` gruplama: system → Microsoft → Third-party → Proje.

## Null-safety
- `<Nullable>enable</Nullable>` tüm projelerde.
- `!` sadece ya ayrıntı ile gerekçelendirilmiş ya da unit testle korunmuş yerde.
- Public API'de `string?` vs `string` ayrımı net.

## Async
- IO / MAUI lifecycle → `async Task`.
- `async void` yalnızca event handler.
- `ConfigureAwait(false)` kütüphane katmanında; UI katmanında gerekmez.
- `CancellationToken` parametre olarak geç; uzun süren işte kontrol et.

## Hata yönetimi
- Exception pattern: beklenmedik durumlar için. Akış kontrolü için exception **yok**.
- Tanımlı hata yolu için `Result<T>` veya discriminated union (record + pattern match).
- Ilgisiz try/catch yasak; swallow yasak.

## Mimari
- MVVM (CommunityToolkit.Mvvm). Page/ViewModel/Model.
- ViewModel'da platform API'sine doğrudan dokunma; `I<X>Service` interface arkasından.
- Singleton yerine DI ile lifecycle yönetimi.
- God class yok; >400 satır class kırmayı düşün.

## Logging
- `Microsoft.Extensions.Logging.ILogger<T>` inject et.
- Debug seviyesi dev-only; Release'de `Information` tavan.
- PII logging yasak.

## Test
- xUnit + `FluentAssertions` (opsiyonel).
- Unit test: pure C# core loop, formüller, validation.
- UI test ship sonrası; ship öncesi smoke manuel.
- Test adlandırma: `Method_State_Expected`.

## Commit disiplini
- Subject imperative, ≤72 char.
- Body neden (what zaten diff'te görünür).
- Atomik commit: tek değişiklik niyeti.
- `--no-verify` yok.
