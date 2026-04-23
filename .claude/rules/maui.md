# .NET MAUI Kuralları

## Proje yapısı
- Tek shared project + `Platforms/Android`, `Platforms/iOS` (iOS kod derlenir ama archive Mac ister).
- Target frameworks: `net10.0-android;net10.0-ios`.
- `$(SupportedOSPlatformVersion)`: Android 24 (API), iOS 15.

## UI seçimi
- **Küçük oyunlar**: `GraphicsView` + `SkiaSharp` (60 FPS custom rendering için).
- UI iskeleti: `Shell` navigation, minimal XAML.
- Karmaşık XAML layout yerine `Grid` + `RelativeLayout`; nested `StackLayout` patlaması yapma.

## MVVM (CommunityToolkit.Mvvm)
- ViewModel: `ObservableObject` base, `[ObservableProperty]`, `[RelayCommand]`.
- Code-behind yalnızca XAML platform-özel tweak için.
- Bindable property yerine CommunityToolkit attribute'ları.

## Servisler
- `IAdService`, `IIapService`, `IStorage`, `IAudio`, `IAnalytics` (ship sonrası opsiyonel) — interface arkasında.
- Platform impl `Platforms/Android|iOS/Services/` altında.
- DI kayıt: `MauiProgram.CreateMauiApp()` içinde.

## Performans reflex'leri
- Page açılışta heavy iş yok — `OnAppearing`'de değil, `OnNavigatedTo` async prefetch.
- Image: raster boyutları hedef cihaz DP'sine göre; @2x/@3x atla, Android drawable-hdpi/mdpi/xhdpi kullan.
- Font: ≤2 font dosyası, subset edilmiş.
- Memory: Page çıkışında event unsubscribe + `IDisposable.Dispose()`.

## Animasyon
- `Animation` API veya Skia custom; Lottie büyük paketli, gerekmedikçe kullanma.
- Animasyon 60 FPS'yi düşürüyorsa (profile), frame count'u kırp.

## Storage
- `sqlite-net-pcl` — Table-per-entity, PK autoincrement, migration için `CreateTable<T>()`.
- DB dosyası `FileSystem.AppDataDirectory` altında.
- `Preferences` sadece küçük flag/config (kullanıcı tercihi, ses seviyesi).

## Ads / IAP
- Android: Google AdMob + Play Billing.
- iOS: AdMob + StoreKit 2.
- **Test modu** dev build'lerde her zaman aktif (test unit ID).

## Lifecycle
- `MainPage`/`Shell` single-activity Android; arka plan geçişi OS standart.
- `Window.Created/Activated/Deactivated/Destroying` event'lerini oyun state'ini pause/resume için kullan.

## Paket boyutu
- AAB hedef ≤40 MB.
- Unused asset'leri `dotnet build -c Release` sonrası trim.
- AOT kullanımı: iOS için zorunlu; Android için default JIT (boyut tercih).
