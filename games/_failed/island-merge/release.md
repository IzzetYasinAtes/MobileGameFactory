# Release Checklist — Mini Kaşifler: Kayıp Adanın Sırrı

**ID**: `island-merge`
**Kapı**: release
**Hedef sürüm**: v1.0.0 (Android only)
**Package name**: `com.mobilegamefactory.minikasifler`
**versionCode**: 1 | **versionName**: 1.0.0

---

## ASO Blogu

### Google Play — Türkçe (birincil)

**Title (30 char)**: `Mini Kaşifler: Kayıp Ada`
Not: "Mini Kaşifler: Kayıp Adanın Sırrı" = 37 karakter; kısaltma zorunlu. "Kayıp Ada" ana keyword'ü tutuyor; marka okunabilir.

**Short description (78 char)**:
`Merge eşyaları, sisi kaldır, gizemli adayı keşfet. Offline çalışır!`

**Long description (4000 char, 3 paragraf)**:

```
Mini Kaşifler: Kayıp Ada, seni gizemli bir adaya düşüren merge bulmaca macerası!
Fog-of-war ile örtülü haritayı açmak için eşyaları birleştir, görevleri tamamla
ve beş büyülü bölgeyi (Tropik Orman → Sahil → Antik Tapınak → Ateş Dağı → Buz
Diyarı) keşfet. Her merge hareketi haritanın bir parçasını aydınlatır; ilerleme
"eşya birleştir" değil, gerçekten "keşfet" hissi verir.

Kaşif Lila ve yardımcı petini (Maymun Momo veya Papağan Rüzgar) seç; enerji
harca, kaynakları topla, aynı eşyaları drag-and-drop ile merge et ve questleri
tamamla. 100 seviye, 5 bölge, her bölgede özgün merge zincirleri: taş, odun,
kristal ve gizli hazine serileri. Kısa seanslar (2-5 dk) tasarlandı; 1 görev,
1 tur, tam tatmin. Arka planda bildirim gönderir — sen yokken üreticiler hazır.

Tamamen çevrimdışı oynanır, internet bağlantısı gerekmez. Zorlayıcı olmayan
reklamlar: rewarded reklam her zaman isteğe bağlı ve net ödüllü ("50 Enerji
kazan"). Seans başında ve core loop ortasında zorunlu reklam yok. Tek sefer
"Reklamsız" seçeneği mevcuttur. Türk kültürel içerikli haftalık etkinlikler:
23 Nisan, Nevruz temalı özel item zincirleri. Google Families Politikası uyumlu.
```

### Google Play — İngilizce (ikincil, v1.0 dahil)

**Title (30 char)**: `Mini Explorers: Lost Island`

**Short description (79 char)**:
`Merge items, lift the fog, explore a mysterious island. Plays fully offline!`

**Long description**: Türkçe long description'ın İngilizce çevirisi (4000 char sınırı içinde, store'da ayrı locale olarak girilir).

### Kategori
- **Android**: Games → Puzzle (primary), Casual (secondary tag)

---

## ASO Anahtar Kelimeleri

### Türkçe — Primary Pool (Google Play description'a doğal yerleşim)
merge oyunu, birleştirme bulmacası, ada keşfi, kayıp ada, sis açma,
puzzle macera, offline oyun, çocuk oyunu, kaşif oyunu, casual merge

### Türkçe — Secondary Pool
fog of war, enerji sistemi, eşya birleştir, quest tamamla, bulmaca

### İngilizce — Primary Pool
merge puzzle, island adventure, fog of war, merge game offline,
casual puzzle, kids merge, explorer game, match and merge, cozy puzzle

### İngilizce — Secondary Pool
merge quest, island mystery, drag and drop, offline adventure, merge items

---

## Screenshot Checklist

| # | Ekran | Mesaj | Çözünürlük (9:16) | Durum |
|---|-------|-------|-------------------|-------|
| 1 | Core board — aktif merge anı | "Birleştir, Keşfet, Kazan!" | 1080×1920 | ☐ |
| 2 | Fog kaldırma animasyonu | "Her merge haritayı açar" | 1080×1920 | ☐ |
| 3 | Quest tamamlama + ödül ekranı | "Görev tamam — ödülünü al" | 1080×1920 | ☐ |
| 4 | Bölge geçişi (Tropik → Sahil) | "5 bölge, 100 seviye" | 1080×1920 | ☐ |
| 5 | Pet seçim ekranı (Momo / Rüzgar) | "Yolculuk arkadaşını seç" | 1080×1920 | ☐ |
| 6 | Rewarded ad opt-in butonu | "İster misin? — Zorunlu değil" | 1080×1920 | ☐ |
| 7 | Tablet — geniş board görünümü | "Büyük ekranda daha fazla keşif" | 1920×1200 (16:9) | ☐ |

Not: Ekran görüntüleri `games/island-merge/assets/screenshots/` altında oluşturulacak. Build tamamlanmadan üretilemez.

---

## Feature Graphic (Android 1024×500)

**Konsept**: Sıcak tropik arka plan (amber-turkuaz degrade), ortada Kaşif Lila + Momo silueti; sağ tarafta fog içinden parlayan merge item yığını. Sol üst: "Mini Kaşifler" logotipi bold TR tipografi, alt satır "Kayıp Ada". Renk paleti: mercan (#FF6B6B), turkuaz (#4ECDC4), amber (#FFE66D). Soft cartoon gölge; karanlık köşe vinyeti yok.

Path: `games/island-merge/assets/feature-graphic-1024x500.png` — ☐

---

## App Icon (512×512 Play Console / 1024×1024 kaynak)

**Konsept**: Sarı-amber arka plan (solid), ortada kaşif şapkalı küçük çocuk yüzü (Lila) — Momo maymun omzunda. Basit emblem stili, fazla detay yok (küçük boyutlarda okunabilir). Soft drop shadow. Adaptive icon: foreground (karakter), background (amber solid).

Path: `games/island-merge/assets/icon-1024x1024.png` — ☐
Path: `games/island-merge/assets/icon-adaptive-fg.png` — ☐
Path: `games/island-merge/assets/icon-adaptive-bg.png` — ☐

---

## Privacy Policy

**URL (placeholder)**: `https://mobilegamefactory.github.io/minikasifler/privacy`

`games/island-merge/privacy.md` oluşturulacak ve statik sayfa olarak yayınlanacak (GitHub Pages önerilen). İçerik: veri toplanmaz (local-only oyun), AdMob SDK cihaz kimliği + IP kullanır, kullanıcı consent banner ile opt-out yapabilir.

**Durum**: ☐ privacy.md yazılacak (build agent tetikleyecek) — submit öncesi ZORUNLU.

---

## Content Rating — Google Play IARC Anket Cevapları

| Soru | Cevap | Gerekçe |
|------|-------|---------|
| Şiddet | Yok | Merge + keşif; savaş yok |
| Cinsellik | Yok | Çocuk dostu görsel dil |
| Korku / gerilim | Çok düşük | Volkan + mağara atmosfer; animasyon hafif |
| Kumar benzeri mekanik | Yok | Loot box yok; rewarded reklam opt-in |
| Sosyal etkileşim | Yok | Offline, çok oyunculu yok |
| Kullanıcı oluşturulan içerik | Yok | — |
| Reklamlar | Evet | AdMob rewarded + interstitial |
| Uygulama içi satın alma | Evet | IAP: energy, remove ads, starter pack |

**Beklenen Rating**: PEGI 7 / ESRB E (Everyone) / Google Play Teen
Gerekçe: Ikincil hedef kitlede 10-17 var; mixed-audience sınıflandırması → store metadata "12+" işaretlenir (monetization.md kararı). Google Families Programı'na katılım reddedildi (v1.0 gelir etkisi); reklam SDK'ya `setTagForUnderAgeOfConsent(true)` gönderilecek.

---

## Data Safety (Google Play)

| Veri Türü | Toplayan | Paylaşılan | Opt-out |
|-----------|----------|------------|---------|
| Device/Advertising ID | AdMob SDK | Google AdMob | Consent banner ile |
| IP adresi | AdMob SDK | Google AdMob | Consent banner ile |
| Purchase history | Google Play Billing | Google | Hayır (platform zorunlu) |
| Crash logs | Yok (local log only) | Hayır | — |
| Konum | Yok | — | — |

Veri şifreleme: transit'te ✓ (HTTPS). Kullanıcı silme talebi: cihaz verisi uygulama kaldırınca silinir ✓.

---

## Submission Checklist — Android v1.0 (Play Console)

- [ ] `keytool` ile `island-merge.keystore` oluştur (`~/keys/island-merge.keystore`)
- [ ] Play App Signing etkinleştir (App integrity)
- [ ] AAB build: `dotnet publish games/island-merge/src/island-merge/island-merge.csproj -f net10.0-android -c Release -p:AndroidPackageFormats=aab`
- [ ] versionCode=1, versionName="1.0.0" doğrula
- [ ] Target SDK = 34 (Android 14), min SDK = 24
- [ ] 64-bit libraries (arm64-v8a) dahil
- [ ] Play Console → New app → Games → Puzzle
- [ ] Store listing yükle (TR + EN)
- [ ] App icon + feature graphic + 7 screenshot yükle
- [ ] Privacy policy URL gir (`https://mobilegamefactory.github.io/minikasifler/privacy`)
- [ ] Data safety formu doldur
- [ ] Content rating questionnaire doldur (IARC)
- [ ] App content → Ads: Yes, IAP: Yes
- [ ] Internal testing'e AAB yükle → 2-3 gün test
- [ ] Production: staged rollout %10 → %25 → %100

---

## iOS Durumu — BLOCKED

iOS submission BLOCKED — Mac erişimi yok, Apple Developer hesabı gerekli.
App Store Connect hazır değil. Xcode archive + IPA imzalama Windows'ta yapılamaz.

**Hedef**: iOS v1.1 — Q3 2026. Öneri: AAB ship sonrası MacInCloud saatlik kiralama (20-40 USD/ay) ile iOS build + TestFlight + App Store Connect upload.

iOS metadata, keywords ve screenshots bu belgedeki Android içeriklerinden uyarlanacak (v1.1 kapısında).

---

## Versioning

- **versionCode**: 1
- **versionName**: 1.0.0
- **Git tag**: `game/island-merge-v1.0.0` (PM ship imzasında atılır)

---

## Launch Planı

- **Hedef tarih**: Build + QA kapıları kapandıktan sonra +7 gün
- **Bölgeler**: Global (TR + EN listing ile; store auto-translate diğer diller)
- **Fiyatlandırma**: Free + IAP (energy_100: 0.99 USD, energy_500: 2.99 USD, starter_pack: 4.99 USD, remove_ads: 3.99 USD)
- **İlk 7 gün monitoring**:
  - Crash rate < %1
  - D1 retention hedef ≥ %30
  - Rating hedef ≥ 4.3
  - Rewarded ad opt-in rate ≥ %25 (Enerji Dolumu placement)
  - P0 review sinyali: günde kontrol

---

## Son Kontrol (PM İmzası)

- [ ] qa.md = GO
- [ ] monetization.md kurallara uyumlu (onaylandı)
- [ ] Privacy policy URL aktif ve erişilebilir
- [ ] Android AAB imzalı ve internal test edildi
- [ ] iOS status: BLOCKED — backlog Q3 2026
- [ ] Store listing TR + EN yazıldı
- [ ] Release notes hazır (TR + EN, ≤500 char)
- [ ] `git tag -a game/island-merge-v1.0.0`

**Eksik (build tamamlanmadan teslim edilemez)**:
1. AAB dosyası (build kapısı bitmedi)
2. Privacy policy URL aktif sayfa
3. Tüm screenshots (7 adet)
4. App icon + feature graphic (final render)

**Karar**: DELAY — yukarıdaki 4 eksik giderilene kadar.

---

## Final Release Package — 2026-04-23

### AAB Durumu — SHIP BLOCKER

**AAB konumu**: `games/island-merge/src/island-merge/bin/Release/net10.0-android/com.mobilegamefactory.islandmerge-Signed.aab`
**Boyut**: ~40.9 MB (hedef ≤40 MB — SINIRDA; trim sonrası kontrol et)

**İmzalama durumu**: DEBUG KEYSTORE — SHIP BLOCKER.
csproj'da `AndroidSigningKeyStore` tanımlı değil. .NET MAUI Release build, keystore belirtilmediğinde Xamarin/MAUI debug keystore (`~/.android/debug.keystore`) ile imzalar. Bu AAB Play Console'a yüklenemez; Play Store yalnızca production keystore ile imzalı AAB kabul eder.

**Blocker çözümü** — owner tarafından yapılacak:

1. Keystore oluştur (JDK keytool mevcut: `C:\Program Files\Android\openjdk\jdk-21.0.8\bin\keytool.exe`):
   ```
   "C:\Program Files\Android\openjdk\jdk-21.0.8\bin\keytool.exe" ^
     -genkey -v ^
     -keystore %USERPROFILE%\keys\island-merge.keystore ^
     -alias islandmerge ^
     -keyalg RSA -keysize 2048 -validity 10000
   ```
2. Keystore'u csproj'a bağla (`Release` PropertyGroup'una ekle):
   ```xml
   <AndroidSigningKeyStore>$(USERPROFILE)\keys\island-merge.keystore</AndroidSigningKeyStore>
   <AndroidSigningKeyAlias>islandmerge</AndroidSigningKeyAlias>
   <AndroidSigningKeyPass>$(KEYSTORE_PASS)</AndroidSigningKeyPass>
   <AndroidSigningStorePass>$(KEYSTORE_PASS)</AndroidSigningStorePass>
   ```
3. Release AAB'yi yeniden derle:
   ```
   dotnet publish games/island-merge/src/island-merge/island-merge.csproj -f net10.0-android -c Release -p:AndroidPackageFormats=aab
   ```
4. Play App Signing'i etkinleştir (Play Console → App integrity) — Google upload key yönetir, production keystore yedeğini güvenli sakla.

**VersionCode**: 1 | **VersionName**: 1.0.0 | **Target SDK**: 34 (API 34) | **Min SDK**: 24

---

### Play Store Asset Checklist

| Asset | Gerekli boyut | Mevcut dosya | Durum |
|-------|---------------|--------------|-------|
| App icon | 512×512 PNG | `assets/icon-512x512.png` (placeholder) | PLACEHOLDER — owner final render gerekli |
| Feature graphic | 1024×500 PNG | `assets/feature-graphic-1024x500.png` (placeholder) | PLACEHOLDER — owner final render gerekli |
| Screenshot 1 — MainMenu | 1080×1920 | `assets/screenshot-mainmenu.png` (1440×753 desktop) | BOYUT YANLIS — portrait crop gerekli |
| Screenshot 2 — CharacterSelect | 1080×1920 | `assets/screenshot-characterselect.png` (placeholder) | PLACEHOLDER |
| Screenshot 3 — BoardPage | 1080×1920 | `assets/screenshot-board.png` (placeholder) | PLACEHOLDER |
| Screenshot 4 — BiomeSelectPage | 1080×1920 | `assets/screenshot-biomeselect.png` (placeholder) | PLACEHOLDER |
| Screenshot 5 — ShopPage | 1080×1920 | `assets/screenshot-shop.png` (placeholder) | PLACEHOLDER |
| Privacy policy URL | HTTPS aktif sayfa | `games/island-merge/privacy.md` (local) | BLOCKER — GitHub Pages host gerekli |
| Content rating | IARC anketi | Cevaplar release.md'de | Owner Play Console'da doldurur |
| Data safety | Play Console formu | Veri tablosu release.md'de | Owner Play Console'da doldurur |
| AAB signed | Production keystore | `com.mobilegamefactory.islandmerge-Signed.aab` | BLOCKER — debug keystore |
| Target API 34 | API 34 | csproj SupportedOSPlatformVersion 24.0 | Kontrol: manifest TargetSdkVersion=34 |
| 64-bit (arm64-v8a) | Zorunlu | Release build dahil | Varsayılan OK |

**Screenshot notu**: Mevcut `screenshot-mainmenu.png` 1440×753 (Windows desktop landscape). Play Store için 1080×1920 portrait gerekli. Placeholder'lar store'a yüklenebilir boyutta oluşturuldu; final görseller device/emulator'dan alınmalı. Android Emülatör (portrait 1080×1920) veya gerçek cihaz önerilir.

---

### Submission Adımları — Android (Google Play Console)

**Ön koşul**: Production keystore oluşturuldu, AAB yeniden imzalandı, privacy.md GitHub Pages'te yayında.

1. **play.google.com/console** → "Uygulama oluştur"
   - Varsayılan dil: Türkçe | Ad: `Mini Kaşifler: Kayıp Ada` | Tür: Oyun | Ücretsiz
2. **Mağaza girişi** → Türkçe + İngilizce listing'leri bu dosyadan yapıştır (ASO Blogu bölümü)
3. **Ana mağaza varlıkları** → App icon, Feature graphic, 5 screenshot yükle
4. **Uygulama içeriği** → Privacy policy URL gir → Data safety doldur → Content rating anketi doldur (IARC) → Reklamlar: Evet, IAP: Evet
5. **Sürüm** → Dahili test → AAB yükle → Sürüm notları (TR/EN):
   - TR: `Mini Kaşifler v1.0.0 — İlk sürüm. Merge bulmaca macerası, 5 bölge, 100 seviye. Çevrimdışı oynanır.`
   - EN: `Mini Explorers v1.0.0 — First release. Merge puzzle adventure, 5 biomes, 100 levels. Fully offline.`
6. **Dahili test** → En az 3 test kullanıcısı → 1-2 gün izle → Crash rate < %1 ise devam
7. **Kapalı test (Closed testing)** → 2024+ Google kuralı: yeni uygulama için kapalı testte 12 gün / 20 tester zorunlu (üretim öncesi)
8. **Aşamalı yayın** → %10 → 3 gün bekle → %25 → 3 gün → %100

---

### Blocker Özeti (Owner Aksiyonları)

| # | Blocker | Etki | Owner adımı |
|---|---------|------|-------------|
| 1 | Production keystore yok | AAB Play Console'a yüklenemez | keytool komutu yukarıda; keystore'u güvenli yedekle |
| 2 | Privacy policy URL aktif değil | Submission reddedilir | `games/island-merge/privacy.md` → GitHub Pages publish |
| 3 | Screenshots placeholder | Mağaza görseli yetersiz | Android Emülatör 1080×1920 portrait'ten gerçek ekran görüntüsü al |
| 4 | Icon + Feature graphic placeholder | Mağaza kalitesi düşük | Tasarımcıya veya AI görüntü aracına ver; brand-keyart.png + character-lila.png kaynak |
| 5 | Kapalı test kullanıcıları | Google 2024+ yeni uygulama kuralı | 20 test kullanıcısı davet et (arkadaş/aile), 12 gün bekle |

**iOS**: BLOCKED — Mac erişimi yok. Hedef Q3 2026.

---

### Windows → iOS Durumu

Mac erişimi yok. Xcode archive + IPA imzalama Windows'ta yapılamaz. iOS v1.1 için öneri: Android v1.0 ship sonrası MacInCloud saatlik kiralama ile Xcode build + TestFlight + App Store Connect upload (~20-40 USD/oturum).

**Karar**: Android v1.0 ship — 3 owner-actionable blocker çözülünce. iOS backlog Q3 2026.
