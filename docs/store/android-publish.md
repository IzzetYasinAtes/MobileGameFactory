# Android Publish Rehberi

## Ön koşullar
- Google Play Console hesabı (tek seferlik 25 USD kayıt ücreti).
- Developer hesabı **bireysel** (küçük bağımsız için) veya **organization**.
- Upload keystore (aşağıda oluşturulacak).
- Privacy policy URL (GitHub Pages, Netlify free tier yeterli).
- App icon 1024×1024, feature graphic 1024×500, screenshots.

## İlk sürüm (adım adım)

### 1. Keystore oluştur (tek sefer)
```bash
keytool -genkey -v -keystore ~/keys/<id>.keystore -alias upload -keyalg RSA -keysize 2048 -validity 10000
```
Keystore + şifre **güvenli yerde** tut (password manager). Kaybedersen Play Console key reset gerekir.

### 2. Play App Signing (önerilir)
Play Console: App integrity → Google Play App Signing'i **etkinleştir**. Upload key ile imzalarsın, Google app-signing key üzerinden final imzalar.

### 3. AAB build
```bash
dotnet publish games/<id>/src/<id>/<id>.csproj \
  -f net10.0-android \
  -c Release \
  -p:AndroidPackageFormats=aab \
  -p:AndroidKeyStore=true \
  -p:AndroidSigningKeyStore=$HOME/keys/<id>.keystore \
  -p:AndroidSigningKeyAlias=upload \
  -p:AndroidSigningKeyPass=env:KEY_PASS \
  -p:AndroidSigningStorePass=env:STORE_PASS
```

Çıktı: `bin/Release/net10.0-android/<id>.aab`.

### 4. Play Console uygulama oluştur
- New app → tür: **Game**, kategori seç.
- Default language: English (US) + TR.
- Free / Paid: **Free** (varsayılan).

### 5. Store listing
- App name (30 char).
- Short description (80 char).
- Full description (4000 char).
- App icon (1024×1024 PNG, alfasız tercih).
- Feature graphic (1024×500, video gerekmez ama istenir).
- Phone screenshots (min 4).
- 7" tablet screenshots (min 2).
- 10" tablet screenshots (min 2).
- Promo video URL (YouTube) — opsiyonel.

### 6. Content rating
- Questionnaire doldur; küçük bir arcade oyun için **IARC 3+** veya **Everyone** tipik.

### 7. Target audience
- Yaş aralığı.
- 13 yaş altı varsa COPPA compliance ek kontroller.

### 8. Data safety
- SDK başına topladığı veri işaretle:
  - AdMob: Advertising/Crash data.
  - Google Play Billing: Purchase data.
- "Data encrypted in transit" ✓, "User can request deletion" ✓ (küçük oyun için cihaz datasıyla).

### 9. App content
- Privacy policy URL.
- Ads: **Yes, this app contains ads**.
- App access: test hesabı gerekmez (login yoksa).
- Government apps: No.
- News app: No.

### 10. Release tracks
- **Internal testing**: ilk ~10 tester, instant.
- **Closed testing** (opsiyonel): 100 tester'a kadar, beta.
- **Production**: public ship.

Öneri sıra:
1. Internal testing'e AAB yükle, 2-3 günlük test.
2. Crash/perf temizse Production'a geç.
3. Staged rollout: %10 → 3 gün → %25 → 3 gün → %100.

### 11. Review + onay
- Google review 3-7 saat (standart), ilk submission 1-7 gün olabilir.
- Ret durumunda sebep Play Console'da; düzelt, resubmit.

## Güncelleme (v1.1.0, v1.2.0)

```bash
# versionCode artır (Directory.Build.props veya .csproj)
dotnet publish ... -p:ApplicationVersion=2 -p:ApplicationDisplayVersion=1.1.0
```

Play Console → Production → Create new release → AAB upload → Release notes (500 char).

## Sık yapılan hatalar

- **versionCode artırmamak**: upload reddedilir.
- **Keystore kaybetmek**: Play App Signing yoksa telafisi yok; varsa Play Console "upload key reset" yapar.
- **ProGuard/trim sorunları**: Release build testte çalışmıyorsa `PublishTrimmed=false` deneyerek izole et.
- **Native library abi eksik**: `RuntimeIdentifiers=android-arm;android-arm64;android-x86;android-x64` (küçük oyun için sadece arm + arm64 yeterli).
- **Data safety eksik**: formdan sonra review gerekebilir.

## Araçlar
- `adb logcat -s Mono:*`: MAUI/C# runtime log.
- `adb install -r <id>.apk`: hızlı test (APK yalnızca dev için).
- Android Studio Profiler: CPU/memory ölçüm.
- `bundletool` (Google): AAB'den APK üretip cihazda test.

## Kritik kontrol listesi (release gate'te)
- [ ] versionCode artırıldı.
- [ ] AAB imzalandı ve build yeşil.
- [ ] 64-bit native libraries (arm64) dahil.
- [ ] Target SDK güncel (son Android sürümü - 1 kabul).
- [ ] Privacy policy URL aktif.
- [ ] Data safety formu dolu.
- [ ] App content → Ads = Yes.
- [ ] Screenshot ve icon final.
- [ ] Release notes yazıldı (TR + EN).
