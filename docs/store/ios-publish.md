# iOS Publish Rehberi

## Ön koşullar (sıkı)
- **macOS cihaz** (Xcode + Command Line Tools). Bulut Mac kabul edilir (MacStadium, MacInCloud, AWS EC2 Mac).
- **Apple Developer Program** hesabı: 99 USD/yıl.
- **Xcode** (güncel sürüm).
- **App Store Connect** erişimi.
- **Certificates / Provisioning profiles**:
  - iOS Distribution certificate (store ship için).
  - App Store provisioning profile (bundle id'ye bağlı).
- Privacy policy URL.

**Windows'tan iOS ship yapılamaz.** MAUI iOS source'u Windows'ta derlenir (intellisense/CI), ancak archive + upload Mac gerektirir.

## Kimlik + sertifikalar (tek sefer)

### 1. App ID (bundle identifier)
Apple Developer → Certificates, Identifiers & Profiles → Identifiers → App IDs → Register:
- Bundle ID: `com.<studio>.<game>` (örn: `com.mobilegamefactory.neonbird`).
- Capabilities: In-App Purchase ✓ (IAP için).

### 2. Distribution certificate
Apple Developer → Certificates → + → iOS Distribution → CSR yükle (Keychain Access'ten Create CSR).

### 3. Provisioning profile
Apple Developer → Profiles → + → App Store → App ID seç → Dist cert seç → indir → Xcode'a drag.

## Build (macOS üzerinde)

### csproj ayarları (shared)
```xml
<PropertyGroup Condition="$(TargetFramework.Contains('-ios'))">
  <ApplicationId>com.mobilegamefactory.neonbird</ApplicationId>
  <ApplicationVersion>1</ApplicationVersion>
  <ApplicationDisplayVersion>1.0.0</ApplicationDisplayVersion>
  <CodesignKey>Apple Distribution: Your Name (TEAMID)</CodesignKey>
  <CodesignProvision>ProfileName</CodesignProvision>
</PropertyGroup>
```

### Archive
```bash
dotnet publish games/<id>/src/<id>/<id>.csproj \
  -f net10.0-ios \
  -c Release \
  -p:RuntimeIdentifier=ios-arm64 \
  -p:ArchiveOnBuild=true
```

Çıktı: `bin/Release/net10.0-ios/ios-arm64/publish/<id>.ipa` + Xcode Organizer'da archive.

### Alternatif: Xcode üzerinden
1. MAUI iOS project → `dotnet build -f net10.0-ios -c Release`.
2. Xcode → Open → seç: `bin/Release/net10.0-ios/<id>.xcarchive`.
3. Organizer → Distribute App → App Store Connect → Upload.

## App Store Connect

### 1. Yeni app
App Store Connect → My Apps → + → New App:
- Platform: iOS.
- Name (30 char).
- Primary language.
- Bundle ID (yukarıda tanımlı).
- SKU (benzersiz string, internal; örn: `NEONBIRD_V1`).

### 2. App information
- Subtitle (30 char).
- Category: Games → subcategory.
- Content rights.
- Age rating questionnaire.

### 3. Pricing and availability
- Free.
- Tüm bölgeler veya seçili.

### 4. App privacy
- Data types collected (SDK başına).
- Amaç (advertising, analytics, product personalization).
- Linked to user? Tracking?

### 5. Prepare for submission (version sayfası)
- Screenshots:
  - 6.5" iPhone (1284×2778 veya 1242×2688).
  - 5.5" iPhone (1242×2208) — legacy, opsiyonel.
  - 12.9" iPad Pro (2048×2732) — iPad destekliyorsa.
- Promotional text (170 char) — post-launch değiştirilebilir.
- Description (4000 char).
- Keywords (100 char, virgülle).
- Support URL.
- Marketing URL (opsiyonel).
- Version: 1.0.0.
- Build: Archive yüklendiğinde listelenir, seç.
- Copyright.

### 6. Submit for review
- App review 1-3 gün (ortalama).
- Reject sebepleri doğrudan ASC'de; net açıklama gelir.

### 7. TestFlight (önerilir, launch öncesi)
- Internal testers: 100'e kadar, instant.
- External testers: 10.000'e kadar, review gerekir (1 gün).
- 7 günlük test (min 1-2 oturum).

## IAP setup

### 1. In-App Purchase kaydet
ASC → Features → In-App Purchases → + → Consumable / Non-Consumable / Subscription.
- Product ID (örn: `com.mobilegamefactory.neonbird.removeads`).
- Reference name.
- Pricing tier.
- Localized display name + description.

### 2. Sandbox testing
- Sandbox account oluştur (ASC → Users and Access → Sandbox Testers).
- iOS cihaza sandbox hesapla giriş.
- Uygulama içinden IAP tetikle, sandbox receipt al.

### 3. Receipt validation (opsiyonel)
Küçük oyun için on-device verification yeterli; server-side validation backend gerektirir (biz backend kullanmıyoruz).

## Ads setup (AdMob)

AdMob dashboard → iOS app ekle → App ID al → MAUI projeye:
- `Info.plist` → `GADApplicationIdentifier`.
- ATT (App Tracking Transparency): `NSUserTrackingUsageDescription` ekle.
- SKAdNetwork IDs: AdMob'un verdiği liste → `SKAdNetworkItems`.

## Yaygın ret sebepleri

- **Privacy policy URL eksik veya bozuk**.
- **ATT prompt'u eksik** (reklam SDK'sı varsa zorunlu).
- **Demo account gerekli** (login varsa — bizde yok).
- **Metadata spam** (keyword stuffing).
- **Screenshot**'ta gerçek gameplay yok.
- **Purchase restore button eksik** (IAP varsa zorunlu).
- **Low app quality** (Apple özel: "not sufficiently unique" red gelebilir).

## Güncelleme

```bash
# ApplicationVersion (build) + ApplicationDisplayVersion (semver) artır
dotnet publish -p:ApplicationVersion=2 -p:ApplicationDisplayVersion=1.1.0 ...
```

ASC → App → + Version → 1.1.0 → Build seç → What's New (4000 char) → Submit.

## Critical checklist (release gate'te)

- [ ] Mac cihaz erişimi var (kendi veya cloud).
- [ ] Apple Developer aktif, 99 USD ödenmiş.
- [ ] Dist certificate + provisioning profile hazır.
- [ ] ApplicationVersion ve DisplayVersion artırıldı.
- [ ] Archive yeşil, IPA imzalandı.
- [ ] App Store Connect'te tüm alanlar dolu.
- [ ] Screenshot'lar hedef cihaz boyutlarında.
- [ ] Privacy nutrition labels doldurulmuş.
- [ ] ATT dialogu test edildi (red durumu da).
- [ ] IAP sandbox test edildi.
- [ ] Review notes (gerekirse): "no login, core loop accessible from launch".

Mac erişimi yoksa: Android'le ship, iOS'u `game_meta_patch(gameId, '{"iosStatus":"blocked_mac_needed"}')` olarak işaretle.
