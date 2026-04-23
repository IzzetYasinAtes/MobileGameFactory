# Performans Kuralları

## Hedefler (ship blocker)
- **Cold start** ≤ 2.0 s (mid-range Android, Release).
- **Frame rate** 60 FPS sabit (min 55 FPS 95. yüzdelik).
- **Memory** ≤ 250 MB peak, ≤ 180 MB idle.
- **Disk** APK/AAB ≤ 40 MB; on-disk install ≤ 120 MB.
- **Battery**: 10 dk sürekli oyun → %5 drain altı (orta seviye cihaz).

## Ölçüm (QA kapısı dayanağı)
- Android Studio Profiler (CPU/Memory/Energy) — QA cihaz matrisinde en az 1 low-end cihazda ölçüm.
- `dotnet-trace`, `PerfView` ihtiyaç halinde.
- Frame time: `Choreographer.FrameCallback` veya SkiaSharp çizim başında/sonunda ölçüm.

## Kod tarafı reflex'ler
- Sıcak yol: allocation minimize; `struct` için `in`/`ref`.
- `List<T>` yerine uzunluğu bilinen yerde `Span<T>` veya sabit array.
- String concat loop içinde → `StringBuilder`.
- LINQ sıcak yolda yasak; for/foreach.
- JSON (de)serialize: `System.Text.Json` source-gen (`JsonSerializerContext`) — reflection'dan kaçın.
- `async Task` yerine küçük senkron iş için senkron.

## Asset tarafı
- Sprite atlas kullan (tekli PNG yük değil).
- Ses: 64–96 kbps mono; SFX için WAV → OGG (sıkıştırılmış ama decode ucuz).
- Font subset (sadece kullanılan glyph'ler).
- Texture boyutu: ekran yoğunluğuna göre maks 2048×2048; 4K asset yok.

## MAUI-özgü
- `CollectionView` virtualization + `CachingStrategy.RecycleElement`.
- `Image.Source` için file cache açık.
- XAML parse maliyetini azalt: karmaşık sayfaları `ContentView` parçalara böl, lazy load.

## SQLite
- WAL mode (MCP server'da da açık).
- Hot query için prepared statement.
- Bulk insert transaction içinde.
- Index yalnızca ölçülmüş sorgu için; gereksiz index boyut şişirir.

## Regresyon koruması
- QA matrisi her ship'te cold start + 5 dk oyun sonrası memory ölçer.
- Rakam design.md'deki hedefi aşarsa: P0 bug, ship NO-GO.

## Anti-patternler
- "Her şey async": overhead artırır, throughput düşürür.
- ViewModel içinde `Task.Run` (UI thread'i zaten yanıt verir, gereksiz ctx switch).
- Debug log Release'e sızması.
- Assembly trimming kapalı Release build (boyut şişer).
