# Windows Geliştirme Gerçeği

Geliştirme host'umuz **Windows**. Bu sayfa neleri yapabileceğimizi ve neleri yapamayacağımızı netleştirir.

## Yapılabilir (Windows'ta tam destek)

### Android geliştirme
- **Her şey** Windows'ta çalışır.
- Android SDK, emulator (AVD), fiziksel cihaz USB debugging.
- MAUI Android build + test + publish (AAB).
- Play Console upload.

### MAUI iOS kaynak kodu
- C# iOS kodu Windows'ta **yazılabilir**, intellisense çalışır.
- `dotnet build -f net10.0-ios` → syntax/compile doğrulaması mümkün, ancak:
  - Simulator çalıştırılamaz.
  - Archive yapılamaz.
  - Signing + store upload mümkün değil.

### Ortak proje
- Shared MAUI project'in %95'i (core loop, ViewModel'lar, servis abstraction'ları) Windows'ta geliştirilir + test edilir (Android veya WinUI üzerinden).

### Windows test (bonus)
- MAUI `net10.0-windows` target ile WinUI desktop build — hızlı iterate için.
- Core loop + UI çalışıyor mu hızlı kontrol.
- **Ship hedefi değil**, sadece dev shortcut.

## Yapılamaz (Windows sınırı)

### iOS archive + submit
Apple toolchain **yalnız macOS'ta** çalışır:
- Xcode (iOS SDK) Windows'ta yok.
- `xcodebuild archive` Mac-specific.
- Signing için macOS Keychain gerekiyor.
- App Store Connect Transporter bazı sürümlerde Windows'ta var ama ipa imzalama Mac gerektirir.

### iOS simulator
Simulator iOS bileşeni, yalnız macOS.

### Xamarin "remote build from Windows" (eski)
Visual Studio Windows + remote Mac build senaryosu .NET 10 MAUI'de önerilmez, stabil değil.

## Mac erişim seçenekleri

### 1. Fiziksel Mac
- Mac mini M2 (~600 USD) + 99 USD/yıl developer = ilk yıl ~700 USD.
- Tek seferlik yatırım, en temiz workflow.

### 2. Cloud Mac
- **MacInCloud**: aylık 20-40 USD.
- **MacStadium**: aylık 80 USD+ (dedicated).
- **AWS EC2 Mac** (mac1.metal): saatlik ~1.08 USD, 24 saat minimum → günlük ~26 USD.

Cloud yaklaşımı: CI/build için ekonomik (haftada 1-2 ship), full-time dev için pahalı.

### 3. GitHub Actions macOS runner
- Ücretsiz kota (public repo'lar için sınırlı, private'ta hızlı tükenir).
- `macos-14` runner + `xcodebuild` script → AAB benzeri otomasyon mümkün.
- Yine de ilk sertifika + provisioning profile bir Mac'te oluşturulmalı; sonra CI'ya upload.

### 4. Hackintosh / macOS VM
- Apple EULA ihlali, önerilmez.
- Yasal + teknik riskler.

## Önerilen strateji

### Faz 1: Windows + Android (şimdi)
- Android Release build yeşil.
- Android Play Store submission.
- İlk gelir / kullanıcı akışı Android'de test.

### Faz 2: Cloud Mac session (ship başı 1-2 saatlik)
- Tek bir ship için MacInCloud saatlik rent.
- Dist cert + profile hazırla (tek seferlik).
- IPA archive + upload.
- Sonraki sürümlerde aynı cert/profile, GitHub Actions'tan auto-build.

### Faz 3: Fiziksel Mac (ölçek büyürse)
- 3+ oyun aktif ship'teyse Mac mini yatırımı geri döner.
- Yerel TestFlight hızlanır.

## PM agent nasıl yönetir

Release kapısında PM:
1. `docs/games/<id>/release.md` içinde "iOS build path" ve "iOS ship path" iki ayrı satır.
2. Mac erişim durumunu kaydet: `game_meta_patch(gameId, '{"iosStatus":"ready"|"blocked_mac_needed"}')`.
3. Sahibe net rapor:
   - "Android v1.0.0 ship-ready."
   - "iOS için Mac erişimi: [durum]. Önerim: [cloud Mac kiralama / Faz 1'de Android-only ship]."
4. Owner kararı beklemez; PM varsayılan olarak **Android ship + iOS backlog** yoluna gider, bunu sahibe bildirir.

## MAUI çapraz platform disiplini

Windows'ta yazılan kod iOS'ta sorunsuz derlensin diye:
- Platform-özel kodu `Platforms/Android|iOS|Windows/` altında izole et.
- Shared project'te **hiç** `#if ANDROID` kalabalığı yok.
- `IAdService`, `IIapService` gibi interface'ler shared'da; impl platform'da.
- Image/font asset'ler shared (MAUI resource sistemi hepsi için işler).
- Reflection-heavy kütüphaneler iOS AOT'ta patlayabilir — kaçın veya `source-gen` kullan.

## Test döngüsü
- Her commit: Android build yeşil + unit testler.
- Haftada 1 (veya ship öncesi): cloud Mac'te iOS build doğrulaması (syntax/link).
- Ship öncesi: cloud Mac'te TestFlight 7 gün.

## Kısa özet
**Android: Windows'ta tam akış. iOS: Windows'ta kod yaz, Mac'te build + ship.** PM bu gerçekle yaşar, sahibe her ship'te netleştirir, Android öncelikli çalışır.
